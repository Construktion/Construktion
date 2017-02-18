namespace Construktion
{
    using System;
    using System.Collections.Generic;
    using Blueprints;

    public class Construktion
    {
        private readonly List<Blueprint> _blueprints = new List<Blueprint>
        {
            new StringPropertyBlueprint(),
            new StringBlueprint(),
            new NumericBlueprint(),
            new CharBlueprint(),
            new GuidBlueprint(),
            new BoolBlueprint(),
            new EnumBlueprint(),
            new ClassBlueprint()
            //guardedblueprint
        };

        public IReadOnlyCollection<Blueprint> Blueprints => _blueprints;

        public Construktion()
        {

        }

        public Construktion(Blueprint blueprint)
        {
            blueprint.ThrowIfNull(nameof(blueprint));

            _blueprints.Insert(0, blueprint);
        }

        public Construktion(BlueprintRegistry registry) 
        {
            registry.ThrowIfNull(nameof(registry));

            _blueprints.InsertRange(0, registry.Blueprints);
        }

        public Construktion(Action<BlueprintRegistry> config)
        {
            var registry = new BlueprintRegistry();

            config(registry);

            _blueprints.InsertRange(0, registry.Blueprints);
        }

        public T Construct<T>()
        {
            return DoConstruct<T>(typeof(T), null);
        }

        public T Construct<T>(Action<T> hardCodes)
        {
            return DoConstruct(typeof(T), hardCodes);
        }

        public object Construct(Type request)
        {
            return DoConstruct<object>(request, null);
        }

        private T DoConstruct<T>(Type request, Action<T> hardCodes)
        {
            var pipeline = new DefaultConstruktionPipeline(_blueprints);

            var result = (T)pipeline.Construct(new ConstruktionContext(request));

            hardCodes?.Invoke(result);

            return result;
        }
    }
}