namespace Construktion.Tests.DefaultBlueprints
{
    using Blueprints;
    using Shouldly;
    using Xunit;

    public class EnumBlueprintTests
    {
        [Fact]
        public void Enum_Is_Not_Null()
        {
            var blueprint = new EnumBlueprint();

            var result = (Gender)blueprint.Build(new ConstruktionContext(typeof(Gender)), Default.Pipeline);

            result.ShouldBeOneOf(Gender.F, Gender.M);
        }

        public enum Gender
        {
            F,
            M
        }
    }
}