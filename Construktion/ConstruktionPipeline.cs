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

        private object Construct(ConstruktionContext requestContext, Blueprint blueprint)
        {
            var depth = _underConstruction.Count(x => requestContext.RequestType == x);

            if (depth > Settings.RecurssionDepth ||
                (depth > 0 && Settings.ThrowOnRecurrsion))
            {
                if (Settings.ThrowOnRecurrsion)
                    throw new Exception($"Recurssion Detected: {requestContext.RequestType.FullName}");

                return default(object);
            }

            _underConstruction.Add(requestContext.RequestType);

            var result = blueprint.Construct(requestContext, this);

            _underConstruction.Remove(requestContext.RequestType);

            return result;
        }
    }
}
