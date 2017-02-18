namespace Construktion.Tests.Blueprints
{
    using global::Construktion.Blueprints;
    using Shouldly;
    using Xunit;

    public class BoolBlueprintTests
    {
        [Fact]
        public void should_alternate_values()
        {
            var blueprint = new BoolBlueprint();

            var result1 = (bool)blueprint.Construct(new ConstruktionContext(typeof(bool)), Pipeline.Default);
            var result2 = (bool)blueprint.Construct(new ConstruktionContext(typeof(bool)), Pipeline.Default);
            var result3 = (bool)blueprint.Construct(new ConstruktionContext(typeof(bool)), Pipeline.Default);

            result1.ShouldBeTrue();
            result2.ShouldBeFalse();
            result3.ShouldBeTrue();
        }
    }
}