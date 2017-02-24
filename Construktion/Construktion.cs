namespace Construktion
{
    using System;
    using Blueprints;

    public class Construktion
    {
        private BlueprintRegistry _registry = new BlueprintRegistry();

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

        public Construktion WithRegistry(BlueprintRegistry registry)
        {
            if (registry == null)
                throw new ArgumentNullException(nameof(registry));

            _registry = registry;
            return this;
        }

        public Construktion WithRegistry(Action<BlueprintRegistry> registry)
        {
            registry(_registry);
            return this;
        }

        public Construktion WithBlueprint(Blueprint blueprint)
        {
            _registry.AddBlueprint(blueprint);
            return this;
        }
    }
}