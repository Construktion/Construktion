namespace Construktion.Tests.SimpleBlueprints
{
    using Blueprints.Simple;
    using Internal;
    using Shouldly;
    using Xunit;

    public class BoolBlueprintTests
    {
        [Fact]
        public void should_alternate_values()
        {
            var blueprint = new BoolBlueprint();

            var result1 = (bool)blueprint.Construct(new ConstruktionContext(typeof(bool)),
                new DefaultConstruktionPipeline());
            var result2 = (bool)blueprint.Construct(new ConstruktionContext(typeof(bool)),
                new DefaultConstruktionPipeline());
            var result3 = (bool)blueprint.Construct(new ConstruktionContext(typeof(bool)),
                new DefaultConstruktionPipeline());

            result1.ShouldBe(true);
            result2.ShouldBe(false);
            result3.ShouldBe(true);
        }
    }
}