namespace Construktion.Tests.Blueprints
{
    using System;
    using global::Construktion.Blueprints;
    using Shouldly;
    using Xunit;

    public class GuidBlueprintTests
    {
        [Fact]
        public void Can_Build_Guid()
        {
            var blueprint = new GuidBlueprint();

            var result = (Guid)blueprint.Build(new BuildContext(typeof(Guid)), Default.Pipeline);

            result.ShouldNotBe(new Guid());
        }
    }
}