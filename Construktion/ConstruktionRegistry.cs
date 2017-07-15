// ReSharper disable PossibleMultipleEnumeration
namespace Construktion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Blueprints;
    using Blueprints.Recursive;
    using Blueprints.Simple;

    public class ConstruktionRegistry
    {
        private readonly List<Blueprint> _defaultBlueprints = Default.Blueprints;

        //need to move all these defaults into a config class
        private int? _enumerableCount;
        private Func<List<ConstructorInfo>, ConstructorInfo> _ctorStrategy;
        private Func<Type, IEnumerable<PropertyInfo>> _propertiesSelector;
        private int? _recurssionLimit;
        private Dictionary<Type, Type> _typeMap { get; } = new Dictionary<Type, Type>();
        private List<Blueprint> _customBlueprints { get; } = new List<Blueprint>();

        public int GetRecurssionDepth() => _recurssionLimit ?? 0;
        public int GetEnumerableCount() => _enumerableCount ?? 3;
        public IEnumerable<Blueprint> GetBlueprints() => _customBlueprints.Concat(_defaultBlueprints).ToList();

        public ConstruktionRegistry()
        {

        }

        public ConstruktionRegistry(Action<ConstruktionRegistry> configure)
        {
            configure(this);
        }

        /// <summary>
        /// Register a blueprint to be added to the pipeline. Will replace
        /// any built-in blueprint that may match
        /// </summary>
        /// <param name="blueprint"></param>
        /// <returns></returns>
        public ConstruktionRegistry AddBlueprint(Blueprint blueprint)
        {
            blueprint.GuardNull();

            _customBlueprints.Add(blueprint);
            return this;
        }

        /// <summary>
        /// Register a blueprint to be added to the pipeline. Will replace
        /// any built-in blueprint that may match
        /// </summary>
        /// <returns></returns>
        public ConstruktionRegistry AddBlueprint<TBlueprint>() where TBlueprint : Blueprint, new()
        {
            _customBlueprints.Add((Blueprint) Activator.CreateInstance(typeof(TBlueprint)));
            return this;
        }

        /// <summary>
        /// Register blueprints to be added to the pipeline. Will replace
        /// any built-in blueprints that may match
        /// </summary>
        /// <returns></returns>
        public ConstruktionRegistry AddBlueprints(IEnumerable<Blueprint> blueprints)
        {
            blueprints.GuardNull();

            _customBlueprints.AddRange(blueprints);
            return this;
        }

        /// <summary>
        /// When TContact is requested, construct TImplementation instead.
        /// Useful when you want to construct a specific implementation whenever an interface is requested. 
        /// </summary>
        /// <typeparam name="TContract">The type to be substituted</typeparam>
        /// <typeparam name="TImplementation">Will be used for substitution</typeparam>
        public ConstruktionRegistry Register<TContract, TImplementation>() where TImplementation : TContract
        {
            _typeMap[typeof(TContract)] = typeof(TImplementation);
            return this;
        }

        /// <summary>
        /// Register an instance that will be supplied whenever the type is requested.  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        public ConstruktionRegistry UseInstance<T>(T instance)
        {
            _customBlueprints.Insert(0, new ScopedBlueprint(typeof(T), instance));
            return this;
        }

        /// <summary>
        /// Adds a blueprint that will use the property's attribute to construct it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public ConstruktionRegistry AddPropertyAttribute<T>(Func<T, object> value) where T : Attribute
        {
            var attributeBlueprint = new PropertyAttributeBlueprint<T>(value);

            _customBlueprints.Add(attributeBlueprint);
            return this;
        }

        /// <summary>
        /// Adds a blueprint that will use the parameter's attribute to construct it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public ConstruktionRegistry AddParameterAttribute<T>(Func<T, object> value) where T : Attribute
        {
            var attributeBlueprint = new ParameterAttributeBlueprint<T>(value);

            _customBlueprints.Add(attributeBlueprint);
            return this;
        }

        /// <summary>
        /// Construct objects using the constructor with the fewest arguments
        /// </summary>
        /// <returns></returns>
        public ConstruktionRegistry UseModestCtor()
        {
            _ctorStrategy = Extensions.ModestCtor;
            return this;
        }

        /// <summary>
        /// Construct objects using the constructor with the most arguments
        /// </summary>
        /// <returns></returns>
        public ConstruktionRegistry UseGreedyCtor()
        {
            _ctorStrategy = Extensions.GreedyCtor;
            return this;
        }

        /// <summary>
        /// Omit properties with private setters
        /// </summary>
        /// <returns></returns>
        public ConstruktionRegistry OmitPrivateSetters()
        {
            _propertiesSelector = Extensions.PropertiesWithPublicSetter;
            return this;
        }

        /// <summary>
        /// Construct properties with private setters
        /// </summary>
        /// <returns></returns>
        public ConstruktionRegistry ConstructPrivateSetters()
        {
            _propertiesSelector = Extensions.PropertiesWithAccessibleSetter;
            return this;
        }

        internal void AddRegistry(ConstruktionRegistry registry)
        {
            _customBlueprints.AddRange(registry._customBlueprints);

            foreach (var map in registry._typeMap)
                _typeMap[map.Key] = map.Value;

            //todo yuck
            _ctorStrategy = registry._ctorStrategy ?? _ctorStrategy ?? Extensions.GreedyCtor;
            _propertiesSelector = registry._propertiesSelector ??_propertiesSelector ?? Extensions.PropertiesWithPublicSetter;
            _enumerableCount = registry._enumerableCount ?? _enumerableCount;
            _recurssionLimit = registry._recurssionLimit ?? _recurssionLimit;

            _defaultBlueprints.Replace(typeof(InterfaceBlueprint), new InterfaceBlueprint(_typeMap));

            _defaultBlueprints.Replace(typeof(NonEmptyCtorBlueprint),
                new NonEmptyCtorBlueprint(_ctorStrategy, _propertiesSelector));

            _defaultBlueprints.Replace(typeof(EmptyCtorBlueprint), new EmptyCtorBlueprint(_propertiesSelector));
        }

        /// <summary>
        /// Return 0 for ints and null for nullable ints ending in "Id". 
        /// </summary>
        public ConstruktionRegistry OmitIds()
        {
            _customBlueprints.Add(new OmitPropertyBlueprint(x => x.EndsWith("Id", StringComparison.Ordinal),
                new List<Type> { typeof(int), typeof(int?) }));
            return this;
        }

        /// <summary>
        /// Specify a convention to omit properties of the specified type.
        /// </summary>
        public ConstruktionRegistry OmitProperties(Func<string, bool> convention, Type propertyType)
        {
            _customBlueprints.Add(new OmitPropertyBlueprint(convention, propertyType));
            return this;
        }

        public ConstruktionRegistry EnumerableCount(int count)
        {
            if (count < 0)
                throw new ArgumentException("Cannot set count less than 0");

            _enumerableCount = count;
            return this;
        }

        /// <summary>
        /// Configure how many levels of recurssion you want to construct 
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        public ConstruktionRegistry RecurssionLimit(int limit)
        {
            _recurssionLimit = limit;
            return this;
        }
    }
}