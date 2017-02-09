namespace Construktion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Blueprints;

    public interface ConstruktionPipeline
    {
        object Build(ConstruktionContext requestContext);
    }

    public class DefaultConstruktionPipeline : ConstruktionPipeline
    {
        private readonly IEnumerable<Blueprint> _blueprints;

        public DefaultConstruktionPipeline(IEnumerable<Blueprint> blueprints)
        {
            blueprints.ThrowIfNull(nameof(blueprints));

            _blueprints = blueprints;
        }

        public object Build(ConstruktionContext requestContext)
        {
            var blueprint = GetBlueprint(requestContext);

            var result = blueprint.Build(requestContext, this);

            return result;
        }

        private Blueprint GetBlueprint(ConstruktionContext requestContext)
        {
            var blueprint = _blueprints.FirstOrDefault(x => x.Matches(requestContext));

            if (blueprint == null)
                throw new Exception($"No Blueprint can be found for {requestContext.RequestType.Name}");

            return blueprint;
        }
    }
}
