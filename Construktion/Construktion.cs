using System;
using System.Collections.Generic;
using System.Reflection;
using Construktion.Blueprints;
using Construktion.Debug;

namespace Construktion
{
    public class Construktion
    {
        private readonly DefaultConstruktionSettings _settings;

        public Construktion()
        {
            _settings = new DefaultConstruktionSettings();
        }

        /// <summary>
        /// Construct an object of the specified type.
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
            return DoConstruct(typeof(T), hardCodes);
        }

        /// <summary>
        /// Construct the type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public object Construct(Type type)
        {
            type.GuardNull();

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
            parameterInfo.GuardNull();

            return DoConstruct<object>(null, null, parameterInfo);
        }

        /// <summary>
        /// Construct an IEnumerable of the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> ConstructMany<T>()
        {
            return ConstructMany<T>(_settings.EnumuerableCount);
        }

        /// <summary>
        /// Construct an IEnumerable with a specific count. Cannot be negative.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="count"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public IEnumerable<T> ConstructMany<T>(int count)
        {
            return ConstructMany<T>(null, count);
        }

        /// <summary>
        /// Construct an IEnumerable with hard codes applied after construction.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hardCodes"></param>
        /// <returns></returns>
        public IEnumerable<T> ConstructMany<T>(Action<T> hardCodes)
        {
            return ConstructMany(hardCodes, _settings.EnumuerableCount);
        }

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
            var result = new DebuggingConstruktion(this).DebuggingConstruct(context, out log);

            return result;
        }

        private T DoConstruct<T>(Type type, Action<T> hardCodes, ParameterInfo parameterInfo = null)
        {
            var context = type != null
                ? new ConstruktionContext(type)
                : new ConstruktionContext(parameterInfo);

            var pipeline = new DefaultConstruktionPipeline(_settings);

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
            registry.GuardNull();

            _settings.Apply(registry);
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

            _settings.Apply(registry);
            return this;
        }

        /// <summary>
        /// Add a blueprint to be used during construction.
        /// </summary>
        /// <param name="blueprint"></param>
        /// <returns></returns>
        public Construktion With(Blueprint blueprint)
        {
            blueprint.GuardNull();

            _settings.Apply(blueprint);
            return this;
        }

        /// <summary>
        /// Add blueprints to be used during construction.
        /// </summary>
        /// <param name="blueprints"></param>
        /// <returns></returns>
        public Construktion With(IEnumerable<Blueprint> blueprints)
        {
            blueprints.GuardNull();

            _settings.Apply(blueprints);
            return this;
        }
        /// <summary>
        /// Inject an object that will be used whenever a value of that type is requested. Injected objects are scoped to a Construktion instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public Construktion Inject(Type type, object value)
        {
            _settings.Inject(type, value);
            return this;
        }

        /// <summary>
        /// Inject an object that will be used whenever a value of that type is requested. Injected objects are scoped to a Construktion instance.
        /// </summary>
        /// <param name="value"></param>
        public Construktion Inject<T>(T value)
        {
            _settings.Inject(typeof(T), value);
            return this;
        }
    }
}