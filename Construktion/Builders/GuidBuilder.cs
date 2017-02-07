namespace Construktion.Builders
{
    using System;

    public class GuidBuilder : Builder
    {
        public bool CanBuild(RequestContext context)
        {
            return context.RequestType == typeof(Guid);
        }

        public object Build(RequestContext context, ConstruktionPipeline pipeline)
        {
            return Guid.NewGuid();
        }
    }
}
