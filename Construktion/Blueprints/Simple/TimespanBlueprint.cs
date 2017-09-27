using System;

namespace Construktion.Blueprints.Simple
{
    public class TimespanBlueprint : AbstractBlueprint<TimeSpan>
    {
        private const int twoDays = 172800;

        public override TimeSpan Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var timespan = new TimeSpan(0, 0, 0, _random.Next(twoDays));

            return timespan;
        }
    }
}