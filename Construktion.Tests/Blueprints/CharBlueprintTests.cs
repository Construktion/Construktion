namespace Construktion.Tests.Blueprints
{
    using global::Construktion.Blueprints;
    using Shouldly;
    using Xunit;

    public class CharBlueprintTests
    {
        [Fact]
        public void not_null()
        {
            var blueprint = new CharBlueprint();

            var result = (char)blueprint.Build(new ConstruktionContext(typeof(char)), Default.Pipeline);

            result.ShouldNotBeNull();
        }
    }
}