namespace Construktion.Tests.Simple
{
    using System;
    using Blueprints.Simple;
    using Shouldly;
    using Xunit;

    public class TimespanBlueprintTests
    {
        [Fact]
        public void should_match_timespan()
        {
            var blueprint = new TimespanBlueprint();

            var matches = blueprint.Matches(new ConstruktionContext(typeof(TimeSpan)));

            matches.ShouldBe(true);
        }

        [Fact]
        public void should_create_timespan()
        {
            var blueprint = new TimespanBlueprint();

            var result = blueprint.Construct(new ConstruktionContext(typeof(TimeSpan)), Default.Pipeline);

            result.ShouldNotBe(null);
        }
    }
}