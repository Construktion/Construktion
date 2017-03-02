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

        private Func<List<ConstructorInfo>, ConstructorInfo> _ctorStrategy = Extensions.GreedyCtor;
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

        public void Register<TContract, TImplementation>() where TImplementation : TContract
        {
            if (!_typeMap.ContainsKey(typeof(TContract)))
                _typeMap[typeof(TContract)] = typeof(TImplementation);
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

        internal void AddRegistry(BlueprintRegistry registry)
        {
            _ctorStrategy = registry._ctorStrategy;

            _customBlueprints.AddRange(registry._customBlueprints);

            foreach (var map in registry._typeMap)
                if (!_typeMap.ContainsKey(map.Key))
                {
                    _typeMap[map.Key] = map.Value;
                }

            var idx = _defaultBlueprints.FindIndex(x => x.GetType() == typeof(NonEmptyCtorBlueprint));

            _defaultBlueprints[idx] = new NonEmptyCtorBlueprint(_typeMap, _ctorStrategy);
        }

        internal List<Blueprint> GetBlueprints()
        {
            return _customBlueprints.Concat(_defaultBlueprints).ToList();
        }

        /// <summary>
        /// Return 0 for int properties ending in "Id"
        /// </summary>
        public void OmitIdProperties()
        {
            _customBlueprints.Add(new OmitIdBlueprint());
        }

        /// <summary>
        /// Define a convention to match "Id" properties. Matches ints.
        /// </summary>
        public void OmitIdProperties(Func<string, bool> convention)
        {
            _customBlueprints.Add(new OmitIdBlueprint(convention, typeof(int)));
        }

        /// <summary>
        /// Define a convention to omit properties of the specified type
        /// </summary>
        public void OmitIdProperties(Func<string, bool> convention, Type propertyType)
        {
            _customBlueprints.Add(new OmitIdBlueprint(convention, propertyType));
        }
    }
}