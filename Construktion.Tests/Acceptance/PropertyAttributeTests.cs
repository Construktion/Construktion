namespace Construktion.Tests.Acceptance
{
    using System;
    using Blueprints.Simple;
    using Shouldly;
    using Xunit;

    public class PropertyAttributeTests
    {
        [Fact]
        public void should_set_value_from_attribute()
        {
            var construktion = new Construktion().AddBlueprint(new AttributeBlueprint<Set>(x => x.Value));

            var foo = construktion.Construct<Foo>();

            foo.Bar.ShouldBe("SetFromAttribute");
            foo.Baz.ShouldNotBe("SetFromAttribute");
        }

        public class Foo
        {
            [Set("SetFromAttribute")]
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
