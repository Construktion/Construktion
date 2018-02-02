namespace Construktion.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Blueprints.Simple;

    public class DefaultConstruktionPipeline : ConstruktionPipeline
    {
        private readonly List<Type> _graph = new List<Type>();
        private readonly List<Blueprint> _blueprints;
        private readonly ConstruktionSettings _settings;

        public ConstruktionSettings Settings => _settings;

        public DefaultConstruktionPipeline() : this(new DefaultConstruktionSettings()) { }

        public DefaultConstruktionPipeline(ConstruktionSettings settings)
        {
            _settings = settings;
            _blueprints = settings.Blueprints.ToList();  
        }

        public object Send(ConstruktionContext context)
        {
            var blueprint = _blueprints.First(x => x.Matches(context));

            var result = Construct(context, blueprint);

            var exitBlueprint = _settings.ExitBlueprints.FirstOrDefault(x => x.Matches(result, context));

            result = exitBlueprint?.Construct(result, this) ?? result;

            return result;
        }

        private object Construct(ConstruktionContext context, Blueprint blueprint)
        {
            if (depthReached() || recursionDetected())
            {
                return _settings.ThrowOnRecurrsion && !depthReached()
                    ? throw new Exception($"Recursion Detected: {context.RequestType.FullName}")
                    : default(object);
            }

            _graph.Add(context.RequestType);

            var result = blueprint.Construct(context, this);

            _graph.Remove(context.RequestType);

            return result;

            bool recursionDetected()
            {
                var depth = _graph.Count(x => context.RequestType == x);

                return depth > _settings.RecurssionDepth || (depth > 0 && _settings.ThrowOnRecurrsion);
            }

            bool depthReached() => _graph.Count > _settings.MaxDepth;
        }

        public void Inject(Type type, object value) => _blueprints.Insert(0, new SingletonBlueprint(type, value));
    }
}