namespace Construktion.Tests
{
    using Blueprints;
    using Shouldly;
    using Xunit;

    public class CharBlueprintTests
    {
        [Fact]
        public void Char_Is_Not_Null()
        {
            var blueprint = new CharBlueprint();

            var result = (char)blueprint.Build(new ConstruktionContext(typeof(char)), Default.Pipeline);

            result.ShouldNotBeNull();
        }
    }
}