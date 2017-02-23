namespace Construktion.Blueprints
{
    using System;
    using System.Reflection;

    public class EnumBlueprint : Blueprint 
    {
        private readonly Random _random = new Random();

        public bool Matches(ConstruktionContext context)
        {
            return context.Request.GetTypeInfo().IsEnum;
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var values = Enum.GetValues(context.Request);

            return values.GetValue(_random.Next(values.Length));
        }
    }
}
