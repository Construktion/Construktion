namespace Construktion.Tests
{
    using System.Linq;
    using System.Reflection;
    using Shouldly;

    public class ConstruktionContextTests
    {
        public void request_type_should_match_request()
        {
            var context = new ConstruktionContext(typeof(string));

            context.RequestType.ShouldBe(typeof(string));

            context.PropertyInfo.ShouldBeNulloPropertyInfo();
            context.ParameterInfo.ShouldBeNulloParameterInfo();
        }

        public void should_set_property_info()
        {
            var context = new ConstruktionContext(typeof(Foo).GetProperty(nameof(Foo.Bar)));

            context.RequestType.ShouldBe(typeof(string));
            context.PropertyInfo.ShouldNotBe(null);

            context.ParameterInfo.ShouldBeNulloParameterInfo();
        }

        public void should_set_parameter_info()
        {
            var parameterInfo = typeof(ConstruktionContextTests)
                .GetMethod(nameof(TestMethod), BindingFlags.NonPublic | BindingFlags.Instance)
                .GetParameters()
                .Single();

            var context = new ConstruktionContext(parameterInfo);

            context.RequestType.ShouldBe(typeof(string));
            context.ParameterInfo.ShouldNotBe(null);

            context.PropertyInfo.ShouldBeNulloPropertyInfo();
        }

        private void TestMethod(string name) { }

        public class Foo
        {
            public string Bar { get; set; }
        }
    }
}