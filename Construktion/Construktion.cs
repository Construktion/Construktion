// ReSharper disable PossibleMultipleEnumeration
namespace Construktion
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Internal;

    public class Construktion
    {
        private readonly DefaultConstruktionSettings _settings;
        private ConstruktionPipeline pipeline;

        public Construktion()
        {
            _settings = new DefaultConstruktionSettings();
            pipeline = new DefaultConstruktionPipeline(_settings);
        }

        /// <summary>
        /// Construct an object of the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Construct<T>() => DoConstruct<T>(typeof(T), null);

        /// <summary>
        /// Construct an object with hard codes to be applied after construction.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hardCodes"></param>
        /// <returns></returns>
        public T Construct<T>(Action<T> hardCodes) => DoConstruct(typeof(T), hardCodes);

        /// <summary>
        /// Construct the type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public object Construct(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            return DoConstruct<object>(type, null);
        }

        /// <summary>
        /// Construct the parameter info.
        /// </summary>
        /// <param name="parameterInfo"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public object Construct(ParameterInfo parameterInfo)
        {
            parameterInfo = parameterInfo ?? throw new ArgumentNullException(nameof(parameterInfo));

            return DoConstruct<object>(null, null, parameterInfo);
        }

        /// <summary>
        /// Construct an IEnumerable of the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> ConstructMany<T>() => ConstructMany<T>(_settings.EnumuerableCount);

        /// <summary>
        /// Construct an IEnumerable with a specific count. Cannot be negative.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="count"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public IEnumerable<T> ConstructMany<T>(int count) => ConstructMany<T>(null, count);

        /// <summary>
        /// Construct an IEnumerable with hard codes applied after construction.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hardCodes"></param>
        /// <returns></returns>
        public IEnumerable<T> ConstructMany<T>(Action<T> hardCodes) => ConstructMany(hardCodes, _settings.EnumuerableCount);

        /// <summary>
        /// Construct an IEnumerable with hard codes applied after construction and with a specific count. Cannot be negative.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hardCodes"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
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

        /// <summary>
        /// Get detailed log information about the way Construktion is constructing your objects. DO NOT use for normal operations. Should be used for ad hoc debugging only.
        /// </summary>
        /// <returns></returns>
        public object DebuggingConstruct(ConstruktionContext context, out string log)
        {
            var pipeline = new DebuggingConstruktionPipeline(_settings);

            var result = pipeline.DebugSend(context, out List<string> debugLog);

            log = string.Join("\n", debugLog);

            return result;
        }

        private T DoConstruct<T>(Type type, Action<T> hardCodes, ParameterInfo parameterInfo = null)
        {
            var context = type != null
                              ? new ConstruktionContext(type)
                              : new ConstruktionContext(parameterInfo);

            var result = (T)pipeline.Send(context);

            hardCodes?.Invoke(result);

            return result;
        }

        /// <summary>
        /// Adds a registry to be used during construction.
        /// </summary>
        /// <param name="registry"></param>
        /// <returns></returns>
        public Construktion With(ConstruktionRegistry registry)
        {
            registry = registry ?? throw new ArgumentNullException(nameof(registry));

            apply(x => x.Apply(registry.Settings));
            return this;
        }

        /// <summary>
        /// Supply a configuration expression to be used during construction.
        /// </summary>
        /// <param name="configure"></param>
        /// <returns></returns>
        public Construktion With(Action<ConstruktionRegistry> configure)
        {
            var registry = new ConstruktionRegistry();

            configure(registry);

            apply(x => x.Apply(registry.Settings));
            return this;
        }

        /// <summary>
        /// Add a custom blueprint to the pipeline.
        /// </summary>
        /// <param name="blueprint"></param>
        /// <returns></returns>
        public Construktion With(Blueprint blueprint)
        {
            blueprint = blueprint ?? throw new ArgumentNullException(nameof(blueprint));

            apply(x => x.Apply(blueprint));
            return this;
        }

        /// <summary>
        /// Add custom blueprints to the pipeline.
        /// </summary>
        /// <param name="blueprints"></param>
        /// <returns></returns>
        public Construktion With(IEnumerable<Blueprint> blueprints)
        {
            blueprints = blueprints ?? throw new ArgumentNullException(nameof(blueprints));

            apply(x => x.Apply(blueprints));
            return this;
        }

        /// <summary>
        /// Add an exit blueprint to the pipeline. These blueprints are called 
        /// at the end of the chain after all regular blueprints. They receive
        /// a fully constructed object and are the final chance to  alter the
        ///  result of an object.   
        /// </summary>
        /// <param name="blueprint"></param>
        /// <returns></returns>
        public Construktion With(ExitBlueprint blueprint)
        {
            blueprint = blueprint ?? throw new ArgumentNullException(nameof(blueprint));

            apply(x => x.Apply(blueprint));
            return this;
        }

        /// <summary>
        /// Inject an object that will be used whenever a value of that type is requested. Injected objects are scoped to a Construktion instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public Construktion Inject(Type type, object value)
        {
            apply(x => x.Inject(type, value));
            return this;
        }

        /// <summary>
        /// Inject an object that will be used whenever a value of that type is requested. Injected objects are scoped to a Construktion instance.
        /// </summary>
        /// <param name="value"></param>
        public Construktion Inject<T>(T value)
        {
            apply(x => x.Inject(typeof(T), value));
            return this;
        }

        private void apply(Action<DefaultConstruktionSettings> configure)
        {
            configure(_settings);
            pipeline = new DefaultConstruktionPipeline(_settings);
        }
    }
}