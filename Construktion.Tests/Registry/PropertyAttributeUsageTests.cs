namespace Construktion.Tests.Registry
{
    using System;
    using Shouldly;
    using Xunit;

    public class PropertyAttributeUsageTests
    {
        [Fact]
        public void should_register_property_attribute_blueprint()
        {
            var registry = new ConstruktionRegistry().AddPropertyAttribute<Set>(x => x.Value);

            var foo = new Construktion().With(registry).Construct<Foo>();

            foo.WithAttribute.ShouldBe("Set");
        }

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