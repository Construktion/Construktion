namespace Construktion.Blueprints.Simple
{
    using System;

    internal class StringPropertyBlueprint : Blueprint
    { 
        private readonly Random _random = new Random();

        public bool Matches(ConstruktionContext context)
        {
            return context.PropertyInfo?.PropertyType == typeof(string);
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var result = context.PropertyInfo.Name + "-" + _random.Next(1, 10000);

            return result;
        }
    }
} 