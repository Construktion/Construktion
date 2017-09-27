using Construktion.Blueprints.Simple;
using Shouldly;
using Xunit;

namespace Construktion.Tests.SimpleBlueprints
{
    public class CharBlueprintTests
    {
        [Fact]
        public void should_not_be_null()
        {
            var blueprint = new CharBlueprint();

            var result = (char)blueprint.Construct(new ConstruktionContext(typeof(char)), new DefaultConstruktionPipeline());

            result.ShouldNotBeNull();
        }
    }
}