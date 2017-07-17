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

    /// <summary>
    /// Base class for configuration settings
    /// </summary>
    public class ConstruktionRegistry
    {
        private readonly List<Blueprint> _defaultBlueprints = Default.Blueprints;

        //need to move all these defaults into a config class
        private int? _enumerableCount;
        private Func<List<ConstructorInfo>, ConstructorInfo> _ctorStrategy;
        private Func<Type, IEnumerable<PropertyInfo>> _propertiesSelector;
        private int? _recurssionLimit;

        private readonly Dictionary<Type, Type> _typeMap = new Dictionary<Type, Type>();
        private readonly List<Blueprint> _customBlueprints = new List<Blueprint>();

        /// <summary>
        /// Get the configured recurssion limit.
        /// </summary>
        /// <returns></returns>
        public int GetRecurssionLimit() => _recurssionLimit ?? 0;

        /// <summary>
        /// Get the configured enumerable count.
        /// </summary>
        /// <returns></returns>
        public int GetEnumerableCount() => _enumerableCount ?? 3;
        
        /// <summary>
        /// Get all configured blueprints. (The pipeline evaluates the blueprints in the returned order)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Blueprint> GetBlueprints() => _customBlueprints.Concat(_defaultBlueprints).ToList();

        public ConstruktionRegistry()
        {

        }

        public ConstruktionRegistry(Action<ConstruktionRegistry> configure)
        {
            configure(this);
        }

        /// <summary>
        /// Register a blueprint to be added to the pipeline.
        /// </summary>
        /// <param name="blueprint"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public ConstruktionRegistry AddBlueprint(Blueprint blueprint)
        {
            blueprint.GuardNull();

            _customBlueprints.Add(blueprint);
            return this;
        }

        /// <summary>
        /// Register a blueprint to be added to the pipeline.
        /// </summary>
        /// <returns></returns>
        public ConstruktionRegistry AddBlueprint<TBlueprint>() where TBlueprint : Blueprint, new()
        {
            _customBlueprints.Add((Blueprint) Activator.CreateInstance(typeof(TBlueprint)));
            return this;
        }

        /// <summary>
        /// Register blueprints to be added to the pipeline.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public ConstruktionRegistry AddBlueprints(IEnumerable<Blueprint> blueprints)
        {
            blueprints.GuardNull();

            _customBlueprints.AddRange(blueprints);
            return this;
        }

        /// <summary>
        /// When the type is requested, construct the subtitute type instead.
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
        /// Register a single instance that will be supplied whenever the type is requested.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        public ConstruktionRegistry UseInstance<T>(T instance)
        {
            _customBlueprints.Insert(0, new ScopedBlueprint(typeof(T), instance));
            return this;
        }

        /// <summary>
        /// Adds a blueprint that will use the property's attribute to construct it.
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
        /// Adds a blueprint that will use the parameter's attribute to construct it.
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
        /// Construct objects using the constructor with the fewest arguments.
        /// </summary>
        /// <returns></returns>
        public ConstruktionRegistry UseModestCtor()
        {
            _ctorStrategy = Extensions.ModestCtor;
            return this;
        }

        /// <summary>
        /// Construct objects using the constructor with the most arguments. This is the default behavior.
        /// </summary>
        /// <returns></returns>
        public ConstruktionRegistry UseGreedyCtor()
        {
            _ctorStrategy = Extensions.GreedyCtor;
            return this;
        }

        /// <summary>
        /// Omit properties with private setters. This is the default behavior.
        /// </summary>
        /// <returns></returns>
        public ConstruktionRegistry OmitPrivateSetters()
        {
            _propertiesSelector = Extensions.PropertiesWithPublicSetter;
            return this;
        }

        /// <summary>
        /// Construct properties with private setters.
        /// </summary>
        /// <returns></returns>
        public ConstruktionRegistry ConstructPrivateSetters()
        {
            _propertiesSelector = Extensions.PropertiesWithAccessibleSetter;
            return this;
        }

        /// <summary>
        /// Return 0 for ints and null for nullable ints ending in "Id"
        /// </summary>
        public ConstruktionRegistry OmitIds()
        {
            _customBlueprints.Add(new OmitPropertyBlueprint(x => x.Name.EndsWith("Id", StringComparison.Ordinal),
                new List<Type> { typeof(int), typeof(int?) }));
            return this;
        }

        /// <summary>
        /// Omit all properties of the specified type.
        /// </summary>
        public ConstruktionRegistry OmitProperties(Type propertyType)
        {
            _customBlueprints.Add(new OmitPropertyBlueprint(x => true, propertyType));
            return this;
        }

        /// <summary>
        /// Specify a convention to omit properties of the specified type.
        /// </summary>
        public ConstruktionRegistry OmitProperties(Func<PropertyInfo, bool> convention, Type propertyType)
        {
            _customBlueprints.Add(new OmitPropertyBlueprint(convention, propertyType));
            return this;
        }

        /// <summary>
        /// Specify a convention to omit properties of the specified types.
        /// </summary>
        public ConstruktionRegistry OmitProperties(Func<PropertyInfo, bool> convention, params Type[]  propertyTypes)
        {
            _customBlueprints.Add(new OmitPropertyBlueprint(convention, propertyTypes));
            return this;
        }

        /// <summary>
        /// Omit all virtual properties.
        /// </summary>
        public void OmitVirtualProperties()
        {
            _customBlueprints.Add(new IgnoreVirtualPropertiesBlueprint());
        }

        /// <summary>
        /// Configure how many items to be returned in an IEnumerable. The default is 3.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throws argument exception when count is less than 0</exception>
        public ConstruktionRegistry EnumerableCount(int count)
        {
            if (count < 0)
                throw new ArgumentException("Count cannot be less than 0");

            _enumerableCount = count;
            return this;
        }

        /// <summary>
        /// Configure how many levels of recurssion to construct. By default recurssive properties are ignored.
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throws argument exception when limit is less than 0</exception>
        public ConstruktionRegistry RecurssionLimit(int limit)
        {
            if (limit < 0)
                throw new ArgumentException("Recurssion limit cannot be less than 0");

            _recurssionLimit = limit;
            return this;
        }

        /// <summary>
        /// Construct properties from a supplied function matching a convention.
        /// </summary>
        /// <param name="convention"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ConstruktionRegistry ConstructPropertyUsing(Func<PropertyInfo, bool> convention, Func<object> value)
        {
            _customBlueprints.Add(new CustomPropertyValueBlueprint(convention, value));
            return this;
        }

        internal void AddRegistry(ConstruktionRegistry registry)
        {
            registry.GuardNull();

            _customBlueprints.AddRange(registry._customBlueprints);

            foreach (var map in registry._typeMap)
                _typeMap[map.Key] = map.Value;

            //todo yuck
            _ctorStrategy = registry._ctorStrategy ?? _ctorStrategy ?? Extensions.GreedyCtor;
            _propertiesSelector = registry._propertiesSelector ?? _propertiesSelector ?? Extensions.PropertiesWithPublicSetter;
            _enumerableCount = registry._enumerableCount ?? _enumerableCount;
            _recurssionLimit = registry._recurssionLimit ?? _recurssionLimit;

            _defaultBlueprints.Replace(typeof(ArrayBlueprint), new ArrayBlueprint(_enumerableCount ?? 3));

            _defaultBlueprints.Replace(typeof(EnumerableBlueprint), new EnumerableBlueprint(_enumerableCount ?? 3));

            _defaultBlueprints.Replace(typeof(InterfaceBlueprint), new InterfaceBlueprint(_typeMap));

            _defaultBlueprints.Replace(typeof(NonEmptyCtorBlueprint),
                new NonEmptyCtorBlueprint(_ctorStrategy, _propertiesSelector));

            _defaultBlueprints.Replace(typeof(EmptyCtorBlueprint), new EmptyCtorBlueprint(_propertiesSelector));
        }
    }
}