namespace Construktion
{
    using System;
    using Blueprints;

    public class Construktion
    {
        private readonly BlueprintRegistry _registry;

        public Construktion() : this(new BlueprintRegistry())
        {
        }

        public Construktion(BlueprintRegistry registry)
        {
            if (registry == null)
                throw new ArgumentNullException(nameof(registry));

            _registry = registry;
        }

        public Construktion(Blueprint blueprint)
        {
            if (blueprint == null)
                throw new ArgumentNullException(nameof(blueprint));

            _registry.AddBlueprint(blueprint);
        }

        public Construktion(Action<BlueprintRegistry> config)
        {
            var registry = new BlueprintRegistry();

            config(registry);

            _registry = registry;
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
            var pipeline = new DefaultConstruktionPipeline(_registry.Blueprints);

            var result = (T)pipeline.Construct(new ConstruktionContext(request));
          
            hardCodes?.Invoke(result);

            return result;
        }
    }
}