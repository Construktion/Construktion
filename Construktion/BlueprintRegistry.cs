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
        private Dictionary<Type, Type> _typeMap { get; } = new Dictionary<Type, Type>();
        private List<Blueprint> _customBlueprints { get; } = new List<Blueprint>();

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

        /// <summary>
        /// Registers an implementation to use for the contract class. 
        /// </summary>
        /// <typeparam name="TContract"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        public void Register<TContract, TImplementation>() where TImplementation : TContract
        {
            _typeMap[typeof(TContract)] = typeof(TImplementation);
        }

        public void Register<T>(T instance)
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

        internal void AddRegistry(BlueprintRegistry registry)
        {
            _customBlueprints.AddRange(registry._customBlueprints);

            foreach (var map in registry._typeMap)
                _typeMap[map.Key] = map.Value;

            var idx = _defaultBlueprints.FindIndex(x => x.GetType() == typeof(NonEmptyCtorBlueprint));

            _ctorStrategy = registry._ctorStrategy ?? _ctorStrategy;

            _defaultBlueprints[idx] = new NonEmptyCtorBlueprint(_typeMap, _ctorStrategy ?? Extensions.GreedyCtor);
        }

        internal List<Blueprint> GetBlueprints()
        {
            return _customBlueprints.Concat(_defaultBlueprints).ToList();
        }

        /// <summary>
        /// Return 0 for int properties ending in "Id". Uses Ordinal comparison. 
        /// </summary>
        public void OmitIds()
        {
            _customBlueprints.Add(new OmitPropertyBlueprint(x => x.EndsWith("Id", StringComparison.Ordinal), typeof(int)));
        }
     
        /// <summary>
        /// Define a convention to omit properties of the specified type.
        /// </summary>
        public void OmitProperties(Func<string, bool> convention, Type propertyType)
        {
            _customBlueprints.Add(new OmitPropertyBlueprint(convention, propertyType));
        }
    }
}