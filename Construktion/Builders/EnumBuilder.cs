namespace Construktion.Builders
{
    using System;
    using System.Reflection;

    public class EnumBuilder : Builder 
    {
        private readonly Random _random = new Random();

        public bool CanBuild(RequestContext context)
        {
            return context.RequestType.GetTypeInfo().IsEnum;
        }

        public object Build(RequestContext context, ConstruktionPipeline pipeline)
        {
            var values = Enum.GetValues(context.RequestType);

            return values.GetValue(_random.Next(values.Length));
        }
    }
}
