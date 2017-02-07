namespace Construktion.Builders
{
    using System;

    public class BoolBuilder : Builder
    {
        private bool value;

        public bool CanBuild(RequestContext context)
        {
            return context.RequestType == typeof(bool);
        }

        public object Build(RequestContext context, ConstruktionPipeline pipeline)
        {
            value = !value;

            return value;
        }
    }
}
