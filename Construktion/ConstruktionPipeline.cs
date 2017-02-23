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

        public DefaultConstruktionPipeline(IEnumerable<Blueprint> blueprints)
        {
            _blueprints = blueprints;
        }

        public object Construct(ConstruktionContext requestContext)
        {
            var blueprint = _blueprints.First(x => x.Matches(requestContext));

            var result = blueprint.Construct(requestContext, this);

            return result;
        }
    }
}
