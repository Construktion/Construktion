namespace Construktion.Tests.SimpleBlueprints
{
    using Blueprints.Simple;
    using Shouldly;
    using Xunit;

    public class CharBlueprintTests
    {
        [Fact]
        public void should_not_be_null()
        {
            var blueprint = new CharBlueprint();

            var result = (char)blueprint.Construct(new ConstruktionContext(typeof(char)),
                new DefaultConstruktionPipeline());

            result.ShouldNotBeNull();
        }
    }
}