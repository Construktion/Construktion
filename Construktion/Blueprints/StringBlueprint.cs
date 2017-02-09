namespace Construktion.Blueprints
{
    using System;

    public class StringBlueprint : Blueprint
    {
        private readonly Random _random = new Random();

        public bool Matches(ConstruktionContext context)
        {
            return context.RequestType == typeof(string);
        }

        public object Build(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var result = context.RequestType.Name + "-" + _random.Next(1, 9999);

            return result;
        }
    }
}