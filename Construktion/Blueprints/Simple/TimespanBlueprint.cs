namespace Construktion.Blueprints.Simple
{
    using System;

    public class TimespanBlueprint : Blueprint
    {
        private readonly Random _random = new Random();
        private const int twoDays = 172800;

        public bool Matches(ConstruktionContext context)
        {
            return context.RequestType == typeof(TimeSpan);
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var timespan = new TimeSpan(0, 0, 0, _random.Next(twoDays));

            return timespan;
        }
    }
}