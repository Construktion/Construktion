using System;

namespace Construktion.Blueprints.Simple
{
    public class DateTimeBlueprint : AbstractBlueprint<DateTime>
    {
        public override DateTime Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var start = DateTime.Today.AddYears(-2);

            var range = (DateTime.Today - start).Days;

            return start.AddDays(_random.Next(range));
        }
    }
}