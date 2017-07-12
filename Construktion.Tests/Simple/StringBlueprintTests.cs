namespace Construktion.Tests.Simple
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

            var result = (string)blueprint.Construct(new ConstruktionContext(typeof(string)), Default.Pipeline);

            result.ShouldStartWith("String-");
        }
    }
}