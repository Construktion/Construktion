using System;
using System.Collections.Generic;
using System.Linq;
using Construktion.Blueprints;

namespace Construktion
{
    public interface ConstruktionPipeline
    {
        /// <summary>
        /// The configured settings.
        /// </summary>
        ConstruktionSettings Settings { get; }

        /// <summary>
        /// Send a request through the pipeline to be constructed.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        object Send(ConstruktionContext context);

        /// <summary>
        /// Inject an object that will be used whenever a value of that type is requested.
        /// </summary>
        /// <param name="requestType"></param>
        /// <param name="value"></param>
        void Inject(Type requestType, object value);
    }

    internal class DefaultConstruktionPipeline : ConstruktionPipeline
    {
        private readonly Construktion _construktion;
        public ConstruktionSettings Settings { get; }

        public DefaultConstruktionPipeline() : this(new Construktion()) { }

        public DefaultConstruktionPipeline(Construktion construktion)
        {
            _construktion = construktion;
            Settings = construktion.Registry.ToSettings();
        }

        public object Send(ConstruktionContext context)
        {
            var blueprint = Settings.Blueprints.First(x => x.Matches(context));

            var result = Construct(context, blueprint);

            return result;
        }

        public void Inject(Type type, object value)
        {
            _construktion.Inject(type, value);
        }

        private readonly List<Type> _underConstruction = new List<Type>();

        private object Construct(ConstruktionContext context, Blueprint blueprint)
        {
            if (RecurssionDetected(context))
            {
                return Settings.ThrowOnRecurrsion
                    ? throw new Exception($"Recursion Detected: {context.RequestType.FullName}")
                    : default(object);
            }

            _underConstruction.Add(context.RequestType);

            var result = blueprint.Construct(context, this);

            _underConstruction.Remove(context.RequestType);

            return result;
        }

        private bool RecurssionDetected(ConstruktionContext context)
        {
            var depth = _underConstruction.Count(x => context.RequestType == x);

            return depth > Settings.RecurssionDepth || (depth > 0 && Settings.ThrowOnRecurrsion);
        }
    }
}
