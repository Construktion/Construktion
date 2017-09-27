using Construktion.Blueprints.Simple;
using Shouldly;
using Xunit;

namespace Construktion.Tests.SimpleBlueprints
{
    public class StringBlueprintTests
    {
        [Fact]
        public void should_construct()
        {
            var blueprint = new StringBlueprint();

            var result = (string)blueprint.Construct(new ConstruktionContext(typeof(string)), new DefaultConstruktionPipeline());

            result.ShouldStartWith("String-");
        }
    }
}