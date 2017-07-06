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

        public void AddBlueprint(Blueprint blueprint)
        {
            if (blueprint == null)
                throw new ArgumentNullException(nameof(blueprint));

            _customBlueprints.Add(blueprint);
        }

        public void AddBlueprint<TBlueprint>() where TBlueprint : Blueprint, new()
        {
            _customBlueprints.Add((Blueprint)Activator.CreateInstance(typeof(TBlueprint)));
        }

        public void AddBlueprints(IEnumerable<Blueprint> blueprints)
        {
            if (blueprints == null)
                throw new ArgumentNullException(nameof(blueprints));

            _customBlueprints.AddRange(blueprints);
        }

        /// <summary>
        /// Registers an implementation to be supplied for the contract class. 
        /// Instances are transient and the last registered will be chosen.
        /// </summary>
        /// <typeparam name="TContract"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        public void Register<TContract, TImplementation>() where TImplementation : TContract
        {
            _typeMap[typeof(TContract)] = typeof(TImplementation);
        }

        public void RegisterScoped<T>(T instance)
        {
            throw new NotImplementedException();
        }

        public void AddAttributeBlueprint<T>(Func<T, object> value) where T : Attribute
        {
            var attributeBlueprint = new AttributeBlueprint<T>(value);

            _customBlueprints.Add(attributeBlueprint);
        }

        public void UseModestCtor()
        {
            _ctorStrategy = Extensions.ModestCtor;
        }

        public void UseGreedyCtor()
        {
            _ctorStrategy = Extensions.GreedyCtor;
        }

        public void OmitPrivateSetters()
        {
            _propertiesSelector = Extensions.PropertiesWithPublicSetter;
        }

        public void ConstructPrivateSetters()
        {
            _propertiesSelector = Extensions.PropertiesWithAccessibleSetter;
        }

        internal void AddRegistry(BlueprintRegistry registry)
        {
            _customBlueprints.AddRange(registry._customBlueprints);

            foreach (var map in registry._typeMap)
                _typeMap[map.Key] = map.Value;

            //todo yuck
            _ctorStrategy = registry._ctorStrategy ?? _ctorStrategy ?? Extensions.GreedyCtor;
            _propertiesSelector = registry._propertiesSelector ?? _propertiesSelector ?? Extensions.PropertiesWithPublicSetter;

            _defaultBlueprints.Replace(typeof(NonEmptyCtorBlueprint), new NonEmptyCtorBlueprint(_typeMap, _ctorStrategy, _propertiesSelector));

            _defaultBlueprints.Replace(typeof(EmptyCtorBlueprint), new EmptyCtorBlueprint(_propertiesSelector));
        }

        /// <summary>
        /// Return 0 for int properties ending in "Id". Uses Ordinal comparison. 
        /// </summary>
        public void OmitIds()
        {
            _customBlueprints.Add(new PropertyOmitterBlueprint(x => x.EndsWith("Id", StringComparison.Ordinal), typeof(int)));
        }
     
        /// <summary>
        /// Define a convention to omit properties of the specified type.
        /// </summary>
        public void OmitProperties(Func<string, bool> convention, Type propertyType)
        {
            _customBlueprints.Add(new PropertyOmitterBlueprint(convention, propertyType));
        }
    }
}