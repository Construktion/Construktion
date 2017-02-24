namespace Construktion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Blueprints;
    using Blueprints.Recursive;
    using Blueprints.Simple;

    public class BlueprintRegistry
    {
        private readonly Dictionary<Type, Type> _typeMap = new Dictionary<Type, Type>();
        private readonly List<Blueprint> _customBlueprints = new List<Blueprint>();

        private readonly Lazy<List<Blueprint>> _blueprints;
        internal IReadOnlyCollection<Blueprint> Blueprints => _blueprints.Value;

        public BlueprintRegistry()
        {
            _blueprints = new Lazy<List<Blueprint>>(GetBlueprints);
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

        private List<Blueprint> GetBlueprints()
        {
            var defaults = Default.Blueprints;

            var idx = defaults.FindIndex(x => x.GetType() == typeof(NonEmptyCtorBlueprint));

            defaults[idx] = new NonEmptyCtorBlueprint(_typeMap);

            return _customBlueprints.Concat(defaults).ToList();
        }
    }
}