namespace Construktion
{
    using System;
    using System.Collections.Generic;
    using Blueprints;
    using Blueprints.Recursive;
    using Blueprints.Simple;

    public class BlueprintRegistry
    {
        private readonly List<Blueprint> _blueprints = new List<Blueprint>();

        internal IReadOnlyCollection<Blueprint> Blueprints => _blueprints;

        public void AddBlueprint(Blueprint blueprint)
        {
            if (blueprint == null)
                throw new ArgumentNullException(nameof(blueprint));

            _blueprints.Add(blueprint);
        }

        public void AddContainerBlueprint(ConstruktionContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            _blueprints.Add(new ConstruktionContainerBlueprint(container));
        }

        public void AddContainerBlueprint(Action<ConstruktionContainer> config)
        {
            var container = new ConstruktionContainer();

            config(container);

            _blueprints.Add(new ConstruktionContainerBlueprint(container));
        }

        public void AddAttributeBlueprint<T>(Func<T, object> value) where T : Attribute
        {
            var attributeBlueprint = new AttributeBlueprint<T>(value);

            _blueprints.Insert(0, attributeBlueprint);
        }
    }
}