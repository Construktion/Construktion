namespace Construktion.Tests.Simple
{
    using System;
    using Blueprints.Simple;
    using Shouldly;
    using Xunit;

    public class GuidBlueprintTests
    {
        [Fact]
        public void should_build_non_empty_guid()
        {
            var blueprint = new GuidBlueprint();

            var result = (Guid)blueprint.Construct(new ConstruktionContext(typeof(Guid)), Default.Pipeline);

            result.ShouldNotBe(new Guid());
        }
    }
}