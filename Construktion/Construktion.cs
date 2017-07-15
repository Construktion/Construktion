namespace Construktion
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Blueprints;

    public class Construktion
    {
        private readonly ConstruktionRegistry _registry = new ConstruktionRegistry();

        /// <summary>
        /// Construct an object of the specified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Construct<T>()
        {
            return DoConstruct<T>(typeof(T), null);
        }

        /// <summary>
        /// Construct an object with hard codes to be applied after construction.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hardCodes"></param>
        /// <returns></returns>
        public T Construct<T>(Action<T> hardCodes)
        {
            hardCodes.GuardNull();

            return DoConstruct(typeof(T), hardCodes);
        }

        /// <summary>
        /// Construct the type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public object Construct(Type type)
        {
            type.GuardNull();

            return DoConstruct<object>(type, null);
        }

        /// <summary>
        /// Construct the parameter info
        /// </summary>
        /// <param name="parameterInfo"></param>
        /// <returns></returns>
        public object Construct(ParameterInfo parameterInfo)
        {
            parameterInfo.GuardNull();

            return DoConstruct<object>(null, null, parameterInfo);
        }

        /// <summary>
        /// Construct an enumerable of the specified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> ConstructMany<T>()
        {
            return ConstructMany<T>(_registry.GetEnumerableCount());
        }

        /// <summary>
        /// Construct an enumerable of the specified type with a certain count
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="count"></param>
        /// <returns></returns>
        public IEnumerable<T> ConstructMany<T>(int count)
        {
            return ConstructMany<T>(null, count);
        }

        /// <summary>
        /// Construct many objects with hard codes applied after construction
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hardCodes"></param>
        /// <returns></returns>
        public IEnumerable<T> ConstructMany<T>(Action<T> hardCodes)
        {
            return ConstructMany(hardCodes, _registry.GetEnumerableCount());
        }

        /// <summary>
        /// Construct many objects with hard codes applied after construction and with a certain count
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hardCodes"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public IEnumerable<T> ConstructMany<T>(Action<T> hardCodes, int count)
        {
            if (count < 0)
                throw new ArgumentException("Cannot set count less than 0");

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

            var pipeline = new DefaultConstruktionPipeline(_registry.GetBlueprints(), _registry.GetRecurssionDepth());

            var result = (T)pipeline.Construct(context);

            hardCodes?.Invoke(result);

            return result;
        }

        /// <summary>
        /// Adds a registy to be used during construction
        /// </summary>
        /// <param name="registry"></param>
        /// <returns></returns>
        public Construktion With(ConstruktionRegistry registry)
        {
            _registry.AddRegistry(registry);
            return this;
        }

        /// <summary>
        /// Supply a configuration expression to be used during construction
        /// </summary>
        /// <param name="configure"></param>
        /// <returns></returns>
        public Construktion With(Action<ConstruktionRegistry> configure)
        {
            var registry = new ConstruktionRegistry();

            configure(registry);

            _registry.AddRegistry(registry);

            return this;
        }

        /// <summary>
        /// Add a blueprint to be used during construction
        /// </summary>
        /// <param name="blueprint"></param>
        /// <returns></returns>
        public Construktion With(Blueprint blueprint)
        {
            _registry.AddBlueprint(blueprint);
            return this;
        }

        /// <summary>
        /// Add blueprints to be used during construction
        /// </summary>
        /// <param name="blueprints"></param>
        /// <returns></returns>
        public Construktion With(IEnumerable<Blueprint> blueprints)
        {
            _registry.AddBlueprints(blueprints);
            return this;
        }
    }
}