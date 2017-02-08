namespace Construktion.Tests
{
    using Builders;
    using Shouldly;
    using Xunit;

    public class CharBuilderTests
    {
        [Fact]
        public void Char_Is_Not_Null()
        {
            var builder = new CharBuilder();

            var result = (char)builder.Build(new RequestContext(typeof(char)), Default.Pipeline);

            result.ShouldNotBeNull();
        }
    }
}