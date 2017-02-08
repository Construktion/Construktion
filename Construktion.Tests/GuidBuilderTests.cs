namespace Construktion.Tests
{
    using System;
    using Builders;
    using Shouldly;
    using Xunit;

    public class GuidBuilderTests
    {
        [Fact]
        public void Can_Build_Guid()
        {
            var builder = new GuidBuilder();

            var result = (Guid)builder.Build(new RequestContext(typeof(Guid)), Default.Pipeline);

            result.ShouldNotBe(new Guid());
        }
    }
}