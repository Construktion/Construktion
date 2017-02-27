namespace Construktion
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Blueprints;
    using Blueprints.Simple;

    public class BlueprintRegistry
    {
        internal Func<List<ConstructorInfo>, ConstructorInfo> CtorStrategy { get; private set; } = Extensions.GreedyCtor;
        internal Dictionary<Type, Type> TypeMap { get; } = new Dictionary<Type, Type>();
        internal List<Blueprint> Blueprints { get; } = new List<Blueprint>();

        public void AddBlueprint(Blueprint blueprint)
        {
            if (blueprint == null)
                throw new ArgumentNullException(nameof(blueprint));

            Blueprints.Add(blueprint);
        }

        public void AddBlueprint<TBlueprint>() where TBlueprint : Blueprint, new()
        {
            Blueprints.Add((Blueprint)Activator.CreateInstance(typeof(TBlueprint)));
        }

        public void Register<TContract, TImplementation>() where TImplementation : TContract
        {
            if (!TypeMap.ContainsKey(typeof(TContract)))
                TypeMap[typeof(TContract)] = typeof(TImplementation);
        }

        public void AddAttributeBlueprint<T>(Func<T, object> value) where T : Attribute
        {
            var attributeBlueprint = new AttributeBlueprint<T>(value);

            Blueprints.Add(attributeBlueprint);
        }

        public void UseModestCtor()
        {
            CtorStrategy = Extensions.ModestCtor;
        }

        internal void AddCustomBlueprints(IEnumerable<Blueprint> blueprints)
        {
            if (blueprints == null)
                throw new ArgumentNullException(nameof(blueprints));

            Blueprints.AddRange(blueprints);
        }
    }
}