using System;
using Construktion.Blueprints.Simple;
using Shouldly;
using Xunit;

namespace Construktion.Tests.SimpleBlueprints
{
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