namespace Construktion.Blueprints
{
    using System;
    using System.Reflection;

    public class EnumBlueprint : Blueprint 
    {
        private readonly Random _random = new Random();

        public bool Matches(BuildContext context)
        {
            return context.RequestType.GetTypeInfo().IsEnum;
        }

        public object Build(BuildContext context, ConstruktionPipeline pipeline)
        {
            var values = Enum.GetValues(context.RequestType);

            return values.GetValue(_random.Next(values.Length));
        }
    }
}
