namespace Construktion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Blueprints;

    public interface ConstruktionPipeline
    {
        /// <summary>
        /// The configured settings.
        /// </summary>
        ConstruktionSettings Settings { get; }

        /// <summary>
        /// Send a request through the pipeline to be constructed.
        /// </summary>
        /// <param name="requestContext"></param>
        /// <returns></returns>
        object Send(ConstruktionContext requestContext);
    }

    internal class DefaultConstruktionPipeline : ConstruktionPipeline
    {
        public ConstruktionSettings Settings { get; }

        public DefaultConstruktionPipeline() : this(new DefaultConstruktionSettings()) { }

        public DefaultConstruktionPipeline(ConstruktionSettings settings)
        {
            Settings = settings;
        }

        public object Send(ConstruktionContext requestContext)
        {
            var blueprint = Settings.Blueprints.First(x => x.Matches(requestContext));

            var result = Construct(requestContext, blueprint);

            return result;
        }

        private readonly List<Type> _underConstruction = new List<Type>();

        private object Construct(ConstruktionContext context, Blueprint blueprint)
        {
            if (RecurssionDetected(context))
            {
                return Settings.ThrowOnRecurrsion
                    ? throw new Exception($"Recurssion Detected: {context.RequestType.FullName}")
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
