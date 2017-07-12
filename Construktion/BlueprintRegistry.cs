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

    public class BlueprintRegistry
    {
        private readonly List<Blueprint> _defaultBlueprints = Default.Blueprints;

        private Func<List<ConstructorInfo>, ConstructorInfo> _ctorStrategy;
        private Func<Type, IEnumerable<PropertyInfo>> _propertiesSelector;
        private Dictionary<Type, Type> _typeMap { get; } = new Dictionary<Type, Type>();
        private List<Blueprint> _customBlueprints { get; } = new List<Blueprint>();

        public IList<Blueprint> ReadBlueprints() => _customBlueprints.Concat(_defaultBlueprints).ToList();

        public BlueprintRegistry()
        {

        }

        public BlueprintRegistry(Action<BlueprintRegistry> configure)
        {
            configure(this);
        }

        public BlueprintRegistry AddBlueprint(Blueprint blueprint)
        {
           blueprint.GuardNull();

            _customBlueprints.Add(blueprint);
            return this;
        }

        public BlueprintRegistry AddBlueprint<TBlueprint>() where TBlueprint : Blueprint, new()
        {
            _customBlueprints.Add((Blueprint)Activator.CreateInstance(typeof(TBlueprint)));
            return this;
        }

        public BlueprintRegistry AddBlueprints(IEnumerable<Blueprint> blueprints)
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
        public BlueprintRegistry Register<TContract, TImplementation>() where TImplementation : TContract
        {
            _typeMap[typeof(TContract)] = typeof(TImplementation);
            return this;
        }

        /// <summary>
        /// Register an instance that will be supplied whenever the type is requested.  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        public BlueprintRegistry RegisterScoped<T>(T instance)
        {
            _customBlueprints.Add(new ScopedBlueprint(typeof(T), instance));
            return this;
        }

        public BlueprintRegistry AddAttributeBlueprint<T>(Func<T, object> value) where T : Attribute
        {
            var attributeBlueprint = new AttributeBlueprint<T>(value);

            _customBlueprints.Add(attributeBlueprint);
            return this;
        }

        public BlueprintRegistry UseModestCtor()
        {
            _ctorStrategy = Extensions.ModestCtor;
            return this;
        }

        public BlueprintRegistry UseGreedyCtor()
        {
            _ctorStrategy = Extensions.GreedyCtor;
            return this;
        }

        public BlueprintRegistry OmitPrivateSetters()
        {
            _propertiesSelector = Extensions.PropertiesWithPublicSetter;
            return this;
        }

        public BlueprintRegistry ConstructPrivateSetters()
        {
            _propertiesSelector = Extensions.PropertiesWithAccessibleSetter;
            return this;
        }

        internal void AddRegistry(BlueprintRegistry registry)
        {
            _customBlueprints.AddRange(registry._customBlueprints);

            foreach (var map in registry._typeMap)
                _typeMap[map.Key] = map.Value;

            //todo yuck
            _ctorStrategy = registry._ctorStrategy ?? _ctorStrategy ?? Extensions.GreedyCtor;
            _propertiesSelector = registry._propertiesSelector ?? _propertiesSelector ?? Extensions.PropertiesWithPublicSetter;

            _defaultBlueprints.Replace(typeof(InterfaceBlueprint), new InterfaceBlueprint(_typeMap));

            _defaultBlueprints.Replace(typeof(NonEmptyCtorBlueprint), new NonEmptyCtorBlueprint(_ctorStrategy, _propertiesSelector));

            _defaultBlueprints.Replace(typeof(EmptyCtorBlueprint), new EmptyCtorBlueprint(_propertiesSelector));
        }

        /// <summary>
        /// Return 0 for ints and null for nullable ints ending in "Id". Uses Ordinal comparison. 
        /// </summary>
        public BlueprintRegistry OmitIds()
        {
            _customBlueprints.Add(new OmitPropertyBlueprint(x => x.EndsWith("Id", StringComparison.Ordinal), new List<Type>{ typeof(int), typeof(int?)}));
            return this;
        }
     
        /// <summary>
        /// Specify a convention to omit properties of the specified type.
        /// </summary>
        public BlueprintRegistry OmitProperties(Func<string, bool> convention, Type propertyType)
        {
            _customBlueprints.Add(new OmitPropertyBlueprint(convention, propertyType));
            return this;
        }
    }
}