namespace Construktion.Tests
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using Shouldly;
    using Xunit;

    public class ConstruktionContextTests
    {
        [Fact]
        public void request_type_should_match_request()
        {
            var context = new ConstruktionContext(typeof(string));

            context.RequestType.ShouldBe(typeof(string));
        }

        [Fact]
        public void should_build_property_context_correctly()
        {
            var context = new ConstruktionContext(typeof(Foo).GetProperty(nameof(Foo.Bar)));

            context.RequestType.ShouldBe(typeof(string));
            context.PropertyContext.Name.ShouldBe("Bar");
            context.PropertyContext.IsType(typeof(string)).ShouldBe(true);
            context.PropertyContext.GetAttributes(typeof(MaxLengthAttribute)).Count().ShouldBe(1);
        }

        [Fact]
        public void should_build_parameter_info_context_correctly()
        {
            var parameterInfo =
                typeof(ConstruktionContextTests).GetMethod(nameof(TestMethod))
                    .GetParameters()
                    .Single();

            var context = new ConstruktionContext(parameterInfo);

            context.RequestType.ShouldBe(typeof(string));
            context.ParameterContext.Name.ShouldBe("name");
            context.ParameterContext.IsType(typeof(string)).ShouldBe(true);
            context.ParameterContext.GetAttributes(typeof(MaxLengthAttribute)).Count().ShouldBe(1);
        }

        public void TestMethod([MaxLength(1)] string name)
        {
            
        }

        public class Foo
        {
            [MaxLength]
            public string Bar { get; set; }
        }
    }
}