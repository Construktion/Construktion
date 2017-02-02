namespace Construktion.Builders
{
    using System;

    public class GuidBuilder : Builder
    {
        public bool CanBuild(Type request)
        {
            return request == typeof(Guid);
        }

        public object Build(RequestContext context)
        {
            return Guid.NewGuid();
        }
    }
}
