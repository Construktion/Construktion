namespace Construktion
{
    using System;
    using System.Reflection;
    using Blueprints;

    public class Construktion
    {
        private readonly BlueprintRegistry _registry = new BlueprintRegistry();

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

        public object Construct(ParameterInfo request)
        {
            return DoConstruct<object>(null, null, request);
        }

        private T DoConstruct<T>(Type type, Action<T> hardCodes, ParameterInfo parameterInfo = null)
        {
            var context = type != null
                ? new ConstruktionContext(type)
                : new ConstruktionContext(parameterInfo);

            var pipeline = new DefaultConstruktionPipeline(_registry.GetBlueprints());

            var result = (T)pipeline.Construct(context);

            hardCodes?.Invoke(result);

            return result;
        }

        public Construktion AddRegistry(BlueprintRegistry registry)
        {
            _registry.ApplyRegistry(registry);
            return this;
        }

        public Construktion AddBlueprint(Blueprint blueprint)
        {
            _registry.AddBlueprint(blueprint);
            return this;
        }
    }
}