namespace Construktion.Tests.SimpleBlueprints
{
    using Blueprints.Simple;
    using Shouldly;
    using Xunit;

    public class StringBlueprintTests
    {
        [Fact]
        public void should_construct()
        {
            var blueprint = new StringBlueprint();

            var result = (string)blueprint.Construct(new ConstruktionContext(typeof(string)),
                new DefaultConstruktionPipeline());

            result.ShouldStartWith("String-");
        }
    }
}