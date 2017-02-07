namespace Construktion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Builders;

    public interface ConstruktionPipeline
    {
        object Build(RequestContext requestContext);
    }

    public class DefaultConstruktionPipeline : ConstruktionPipeline
    {
        private readonly IEnumerable<Builder> _builders;

        public DefaultConstruktionPipeline(IEnumerable<Builder> builders)
        {
            if (builders == null)
                throw new ArgumentNullException(nameof(builders));  

            _builders = builders;
        }

        public object Build(RequestContext requestContext)
        {
            var builder = _builders.FirstOrDefault(x => x.CanBuild(requestContext));

            var result = builder.Build(requestContext, this);

            return result;
        }
    }
}
