namespace Construktion.Blueprints
{
    using System;

    public class StringPropertyBlueprint : Blueprint
    { 
        private readonly Random _random = new Random();

        public bool Matches(ConstruktionContext context)
        {
            return context.PropertyContext.IsType(typeof(string));
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var result = context.PropertyContext.Name + "-" + _random.Next(1, 10000);

            return result;
        }
    }
} 