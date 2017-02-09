namespace Construktion.Builders
{
    using System;
    using System.Reflection;

    public class EnumBuilder : Builder 
    {
        private readonly Random _random = new Random();

        public bool CanBuild(ConstruktionContext context)
        {
            return context.RequestType.GetTypeInfo().IsEnum;
        }

        public object Build(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var values = Enum.GetValues(context.RequestType);

            return values.GetValue(_random.Next(values.Length));
        }
    }
}
