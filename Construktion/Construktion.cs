namespace Construktion
{
    using System;
    using System.Collections.Generic;
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
            hardCodes.GuardNull();

            return DoConstruct(typeof(T), hardCodes);
        }

        public object Construct(Type type)
        {
            type.GuardNull();

            return DoConstruct<object>(type, null);
        }

        public object Construct(ParameterInfo parameterInfo)
        {
            parameterInfo.GuardNull();

            return DoConstruct<object>(null, null, parameterInfo);
        }

        public IEnumerable<T> ConstructMany<T>()
        {
            return ConstructMany<T>(_registry.GetEnumerableCount());
        }

        public IEnumerable<T> ConstructMany<T>(int count)
        {
            return ConstructMany<T>(null, count);
        }

        public IEnumerable<T> ConstructMany<T>(Action<T> hardCodes)
        {
            return ConstructMany(hardCodes, _registry.GetEnumerableCount());
        }

        public IEnumerable<T> ConstructMany<T>(Action<T> hardCodes, int count)
        {
            var items = new List<T>();

            for (var i = 0; i < count; i++)
            {
                var item = DoConstruct<T>(typeof(T), null);

                hardCodes?.Invoke(item);

                items.Add(item);
            }

            return items;
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
            _registry.AddRegistry(registry);
            return this;
        }

        public Construktion AddRegistry(Action<BlueprintRegistry> configure)
        {
            var registry = new BlueprintRegistry();

            configure(registry);

            _registry.AddRegistry(registry);
            return this;
        }

        public Construktion AddBlueprint(Blueprint blueprint)
        {
            _registry.AddBlueprint(blueprint);
            return this;
        }

        public void SetEnumerableCount(int count)
        {
            _registry.EnumerableCount(count);
        }
    }
}