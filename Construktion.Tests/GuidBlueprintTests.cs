namespace Construktion.Tests
{
    using System;
    using Blueprints;
    using Shouldly;
    using Xunit;

    public class GuidBlueprintTests
    {
        [Fact]
        public void Can_Build_Guid()
        {
            var blueprint = new GuidBlueprint();

            var result = (Guid)blueprint.Construct(new ConstruktionContext(typeof(Guid)), Default.Pipeline);

            result.ShouldNotBe(new Guid());
        }
    }
}