namespace Construktion.Tests
{
    using Builders;
    using Shouldly;
    using Xunit;

    public class BoolBuilderTests
    {
        [Fact]
        public void Should_Alternate_Values()
        {
            var builder = new BoolBuilder();

            var result1 = (bool)builder.Build(new ConstruktionContext(typeof(bool)), Default.Pipeline);
            var result2 = (bool)builder.Build(new ConstruktionContext(typeof(bool)), Default.Pipeline);
            var result3 = (bool)builder.Build(new ConstruktionContext(typeof(bool)), Default.Pipeline);

            result1.ShouldBeTrue();
            result2.ShouldBeFalse();
            result3.ShouldBeTrue();
        }
    }
}