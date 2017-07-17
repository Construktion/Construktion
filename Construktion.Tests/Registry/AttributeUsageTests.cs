namespace Construktion.Tests.Registry
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Shouldly;
    using Xunit;
    using Xunit.Sdk;

    public class AttributeUsageTests
    {
        private readonly ConstruktionRegistry _registry;
        private readonly Construktion _construktion;

        public AttributeUsageTests()
        {
            _registry = new ConstruktionRegistry();
            _construktion = new Construktion();
        }

        [Fact]
        public void should_register_property_attribute_blueprint()
        {
            _registry.AddPropertyAttribute<Set>(x => x.Value);

            var foo = _construktion.With(_registry).Construct<Foo>();

            foo.WithAttribute.ShouldBe("Set");
        }

        [Fact]
        public void should_register_parameter_attribute_blueprint()
        {
            _registry.AddParameterAttribute<Set>(x => x.Value);

            var parameterInfo =
                typeof(AttributeUsageTests).GetMethod(nameof(ParameterTest))
                    .GetParameters()
                    .Single();

            var parameter = (string)_construktion.With(_registry).Construct(parameterInfo);

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
