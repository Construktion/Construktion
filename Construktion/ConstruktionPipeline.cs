namespace Construktion
{
    using System.Collections.Generic;
    using System.Linq;
    using Blueprints;

    public interface ConstruktionPipeline
    {
        object Construct(ConstruktionContext requestContext);
    }

    public class DefaultConstruktionPipeline : ConstruktionPipeline
    {
        private readonly IEnumerable<Blueprint> _blueprints;
        private readonly List<object> _recurssionGuard = new List<object>();

        public DefaultConstruktionPipeline(IEnumerable<Blueprint> blueprints)
        {
            _blueprints = blueprints;
        }

        public object Construct(ConstruktionContext requestContext)
        {
            var blueprint = _blueprints.First(x => x.Matches(requestContext));

            var result = construct(requestContext, blueprint);

            return result;
        }

        private object construct(ConstruktionContext requestContext, Blueprint blueprint)
        {
            if (_recurssionGuard.Contains(requestContext.RequestType))
                return default(object);

            _recurssionGuard.Add(requestContext.RequestType);

            var result = blueprint.Construct(requestContext, this);

            _recurssionGuard.Remove(requestContext.RequestType);

            return result;
        }
    }
}
