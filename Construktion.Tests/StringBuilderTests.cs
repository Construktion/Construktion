namespace Construktion.Tests
{
    using Builders;
    using Shouldly;
    using Xunit;

    public class StringBuilderTests
    {
        [Fact]
        public void Can_Build_A_String()
        {
            var builder = new StringBuilder();

            var result = (string)builder.Build(new RequestContext(typeof(string)), Default.Pipeline);

            result.Substring(0, 7).ShouldBe("String-");
        }
    }
}