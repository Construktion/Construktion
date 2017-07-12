namespace Construktion.Tests.Simple
{
    using System;
    using Blueprints.Simple;
    using Shouldly;
    using Xunit;

    public class DateTimeBlueprintTests
    {       
        [Fact]
        public void should_construct()
        {
            var blueprint = new DateTimeBlueprint();

            var result = blueprint.Construct(new ConstruktionContext(typeof(DateTime)), Default.Pipeline);

            result.ShouldNotBe(default(DateTime));
        }
    }
}