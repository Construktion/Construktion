namespace Construktion.Tests
{
    using System.ComponentModel.DataAnnotations;
    using Shouldly;
    using Xunit;

    public class AttributeBasedTests
    {
        [Fact]
        public void Can_Resolve_Based_On_Attributes()
        {
            var construktion = new Construktion();

            var result = construktion.Build<WithMaxLengthAttribute>();

            result.Name.ShouldBe("attr");
        }

        public class WithMaxLengthAttribute
        {
            [MaxLength(12)]
            public string Name { get; set; }
        }
    }
}
