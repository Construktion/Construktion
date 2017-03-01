namespace Construktion.Blueprints.Simple
{
    using System;

    public class DateTimeBlueprint : Blueprint
    {
        private readonly Random _random = new Random();

        public bool Matches(ConstruktionContext context)
        {
            return context.RequestType == typeof(DateTime);
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var start = DateTime.Today.AddYears(-2);

            var range = (DateTime.Today - start).Days;

            return start.AddDays(_random.Next(range));
        }
    }
}