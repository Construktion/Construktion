namespace Construktion.Blueprints
{
    using System;
    using System.Linq;

    public class StringPropertyBlueprint : Blueprint
    {
        private readonly Random _random = new Random();

        public bool Matches(ConstruktionContext context)
        {
            return context.PropertyInfo.HasValue() && context.RequestType == typeof(string);
        }

        public object Build(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var result = context.PropertyInfo.Single().Name + "-" + _random.Next(1, 10000);

            return result;
        }
    }
} 