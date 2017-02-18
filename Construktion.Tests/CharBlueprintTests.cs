namespace Construktion.Tests
{
    using Blueprints;
    using Shouldly;
    using Xunit;

    public class CharBlueprintTests
    {
        [Fact]
        public void not_null()
        {
            var blueprint = new CharBlueprint();

            var result = (char)blueprint.Construct(new ConstruktionContext(typeof(char)), Pipeline.Default);

            result.ShouldNotBeNull();
        }
    }
}