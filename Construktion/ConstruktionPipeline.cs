namespace Construktion
{
    using System;
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
            if (blueprints == null) 
                throw new ArgumentNullException(nameof(blueprints));

            _blueprints = blueprints;
        }

        public object Construct(ConstruktionContext requestContext)
        {
            var blueprint = _blueprints.FirstOrDefault(x => x.Matches(requestContext));

            if (blueprint == null)
                throw new Exception($"No Blueprint can be found for {requestContext.Request.FullName}");

            var result = blueprint.Construct(requestContext, this);

            return result;
        }
    }
}
