namespace Construktion.Builders
{
    using System;

    public class StringBuilder : Builder
    {
        private readonly Random _random = new Random();

        public bool CanBuild(RequestContext context)
        {
            return context.RequestType == typeof(string);
        }

        public object Build(RequestContext context, ConstruktionPipeline pipeline)
        {
            var result = context.RequestType.Name + "-" + _random.Next(1, 9999);

            return result;
        }
    }
}