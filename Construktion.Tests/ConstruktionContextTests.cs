namespace Construktion.Tests
{
    using System.Linq;
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
        public void should_set_property_info()
        {
            var context = new ConstruktionContext(typeof(Foo).GetProperty(nameof(Foo.Bar)));

            context.RequestType.ShouldBe(typeof(string));
            context.PropertyInfo.ShouldNotBe(null);
        }

        [Fact]
        public void should_set_parameter_info()
        {
            var parameterInfo =
                typeof(ConstruktionContextTests).GetMethod(nameof(TestMethod))
                                                .GetParameters()
                                                .Single();

            var context = new ConstruktionContext(parameterInfo);

            context.RequestType.ShouldBe(typeof(string));
            context.ParameterInfo.ShouldNotBe(null);
        }

        public void TestMethod(string name) { }

        public class Foo
        {
            public string Bar { get; set; }
        }
    }
}