using Construktion.Blueprints.Simple;
using Shouldly;
using Xunit;

namespace Construktion.Tests.SimpleBlueprints
{
    public class BoolBlueprintTests
    {
        [Fact]
        public void should_alternate_values()
        {
            var blueprint = new BoolBlueprint();

            var result1 = (bool)blueprint.Construct(new ConstruktionContext(typeof(bool)), new DefaultConstruktionPipeline());
            var result2 = (bool)blueprint.Construct(new ConstruktionContext(typeof(bool)), new DefaultConstruktionPipeline());
            var result3 = (bool)blueprint.Construct(new ConstruktionContext(typeof(bool)), new DefaultConstruktionPipeline());

            result1.ShouldBe(true);
            result2.ShouldBe(false);
            result3.ShouldBe(true);
        }
    }
}