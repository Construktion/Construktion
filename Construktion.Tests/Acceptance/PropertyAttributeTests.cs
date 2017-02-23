namespace Construktion.Tests.Acceptance
{
    using System;
    using Shouldly;
    using Xunit;

    public class PropertyAttributeTests
    {
        [Fact]
        public void should_set_value_from_attribute()
        {
            var construktion = new Construktion(x => x.AddAttributeBlueprint<Set>(a => a.Value));

            var result = construktion.Construct<Foo>();

            result.Bar.ShouldBe("Set");
            result.Baz.ShouldNotBe("Set");
            result.Baz.ShouldNotBeNullOrWhiteSpace();
        }

        public class Foo
        {
            [Set("Set")]
            public string Bar { get; set; }

            public string Baz { get; set; }
        }

        public class Set : Attribute
        {
            public object Value { get; }

            public Set(object value)
            {
                Value = value;
            }
        }
    }
}
