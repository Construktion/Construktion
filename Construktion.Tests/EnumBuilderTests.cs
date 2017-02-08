namespace Construktion.Tests
{
    using Builders;
    using Shouldly;
    using Xunit;

    public class EnumBuilderTests
    {
        [Fact]
        public void Enum_Is_Not_Null()
        {
            var builder = new EnumBuilder();

            var result = (Gender)builder.Build(new RequestContext(typeof(Gender)), Default.Pipeline);

            result.ShouldBeOneOf(Gender.F, Gender.M);
        }

        public enum Gender
        {
            F,
            M
        }
    }
}