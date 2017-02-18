namespace Construktion.Tests
{
    using Blueprints;
    using Shouldly;
    using Xunit;

    public class CharBlueprintTests
    {
        [Fact]
        public void should_not_be_null()
        {
            var blueprint = new CharBlueprint();

            var result = (char)blueprint.Construct(new ConstruktionContext(typeof(char)), Default.Pipeline);

            result.ShouldNotBeNull();
        }
    }
}