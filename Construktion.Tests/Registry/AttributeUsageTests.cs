namespace Construktion.Tests.Registry
{
    using System;
    using System.Linq;
    using Shouldly;
    using Xunit;

    public class AttributeUsageTests
    {
        private readonly ConstruktionRegistry registry;
        private readonly Construktion construktion;

        public AttributeUsageTests()
        {
            registry = new ConstruktionRegistry();
            construktion = new Construktion();
        }

        [Fact]
        public void should_register_property_attribute_blueprint()
        {
            registry.AddPropertyAttribute<Set>(x => x.Value);

            var foo = construktion.With(registry).Construct<Foo>();

            foo.WithAttribute.ShouldBe("Set");
        }

        [Fact]
        public void should_register_parameter_attribute_blueprint()
        {
            registry.AddParameterAttribute<Set>(x => x.Value);

            var parameterInfo =
                typeof(AttributeUsageTests).GetMethod(nameof(ParameterTest))
                    .GetParameters()
                    .Single();

            var parameter = (string)construktion.With(registry).Construct(parameterInfo);

            parameter.ShouldBe("Set");
        }

        public void ParameterTest([Set("Set")] string parameter) { }

        public class Foo
        {
            [Set("Set")]
            public string WithAttribute { get; set; }
        }

        public class Set : Attribute
        {
            public string Value { get; }

            public Set(string value)
            {
                Value = value;
            }
        }
    }
}
