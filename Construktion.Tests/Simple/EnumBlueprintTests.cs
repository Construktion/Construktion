namespace Construktion.Tests.Simple
{
    using Blueprints.Simple;
    using Shouldly;
    using Xunit;

    public class EnumBlueprintTests
    {
        [Fact]
        public void should_not_be_null()
        {
            var blueprint = new EnumBlueprint();

            var result = (Gender)blueprint.Construct(new ConstruktionContext(typeof(Gender)), Default.Pipeline);

            result.ShouldBeOneOf(Gender.F, Gender.M);
        }

        public enum Gender
        {
            F,
            M
        }
    }
}