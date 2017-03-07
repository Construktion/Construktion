namespace Construktion.Tests.Simple
{
    using System;
    using Blueprints.Simple;
    using Shouldly;
    using Xunit;

    public class DateTimeBlueprintTests
    {
        [Fact]
        public void should_match_datetime()
        {
            var blueprint = new DateTimeBlueprint();

            var matches = blueprint.Matches(new ConstruktionContext(typeof(DateTime)));

            matches.ShouldBe(true);
        }

        [Fact]
        public void should_construct_datetime()
        {
            var blueprint = new DateTimeBlueprint();

            var result = blueprint.Construct(new ConstruktionContext(typeof(DateTime)), Default.Pipeline);

            result.ShouldNotBe(default(DateTime));
        }
    }
}