using System;
using System.Collections.Generic;
using System.Linq;
using Construktion.Blueprints;

namespace Construktion
{
    internal class DefaultConstruktionPipeline : ConstruktionPipeline
    {
        //todo remove circular reference #
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
