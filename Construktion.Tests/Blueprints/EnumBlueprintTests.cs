namespace Construktion.Tests.Blueprints
{
    using global::Construktion.Blueprints;
    using Shouldly;
    using Xunit;

    public class EnumBlueprintTests
    {
        [Fact]
        public void not_null()
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