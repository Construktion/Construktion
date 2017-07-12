namespace Construktion.Blueprints.Simple
{
    using System;

    public class TimespanBlueprint : AbstractBlueprint<TimeSpan>
    {
        private const int twoDays = 172800;

        public override object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var timespan = new TimeSpan(0, 0, 0, _random.Next(twoDays));

            return timespan;
        }
    }
}