namespace Construktion
{
    using System;
    using System.Collections.Generic;
    using Blueprints;

    public class BlueprintRegistry
    {
        private readonly List<Blueprint> _blueprints = new List<Blueprint>();

        public IReadOnlyCollection<Blueprint> Blueprints => _blueprints;

        public void AddBlueprint(Blueprint blueprint)
        {
            if (blueprint == null)
                throw new ArgumentNullException(nameof(blueprint));

            _blueprints.Add(blueprint);
        }

        public void AddContainerBlueprint(SimpleContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            _blueprints.Add(new SimpleContainerBlueprint(container));
        }

        public void AddContainerBlueprint(Action<SimpleContainer> config)
        {
            var container = new SimpleContainer();

            config(container);

            _blueprints.Add(new SimpleContainerBlueprint(container));
        }

        public void AddAttributeBlueprint<T>(Func<T, object> value) where T : Attribute
        {
            var attributeBlueprint = new AttributeBlueprint<T>(value);

            _blueprints.Insert(0, attributeBlueprint);
        }
    }
}