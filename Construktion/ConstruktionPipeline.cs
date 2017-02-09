namespace Construktion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Builders;

    public interface ConstruktionPipeline
    {
        object Build(ConstruktionContext requestContext);
    }

    public class DefaultConstruktionPipeline : ConstruktionPipeline
    {
        private readonly IEnumerable<Builder> _builders;

        public DefaultConstruktionPipeline(IEnumerable<Builder> builders)
        {
            builders.ThrowIfNull(nameof(builders));

            _builders = builders;
        }

        public object Build(ConstruktionContext requestContext)
        {
            var builder = GetBuilder(requestContext);

            var result = builder.Build(requestContext, this);

            return result;
        }

        private Builder GetBuilder(ConstruktionContext requestContext)
        {
            var builder = _builders.FirstOrDefault(x => x.CanBuild(requestContext));

            if (builder == null)
                throw new Exception($"No builder can be found for {requestContext.RequestType.Name}");

            return builder;
        }
    }
}
