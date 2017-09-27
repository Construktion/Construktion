using System;
using Construktion.Blueprints.Simple;
using Shouldly;
using Xunit;

namespace Construktion.Tests.SimpleBlueprints
{
    public class GuidBlueprintTests
    {
        [Fact]
        public void should_construct()
        {
            var blueprint = new GuidBlueprint();

            var result = (Guid)blueprint.Construct(new ConstruktionContext(typeof(Guid)), new DefaultConstruktionPipeline());

            result.ShouldNotBe(new Guid());
        }
    }
}