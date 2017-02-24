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
            var construktion = new Construktion().WithRegistry(x => x.AddAttributeBlueprint<Set>(a => a.Value));

            var foo = construktion.Construct<Foo>();

            foo.Bar.ShouldBe("Set");
            foo.Baz.ShouldNotBe("Set");
            foo.Baz.ShouldNotBeNullOrWhiteSpace();
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
