namespace Construktion.Tests.Simple
{
    using System;
    using Blueprints.Simple;
    using Shouldly;
    using Xunit;

    public class TimespanBlueprintTests
    {       
        [Fact]
        public void should_construct()
        {
            var blueprint = new TimespanBlueprint();

            var result = blueprint.Construct(new ConstruktionContext(typeof(TimeSpan)), new DefaultConstruktionPipeline());

            result.ShouldNotBe(default(TimeSpan));
        }
    }
}