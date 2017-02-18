namespace Construktion.Tests
{
    using System;
    using System.Reflection;
    using Blueprints;
    using Shouldly;
    using Xunit;

    public class AttributeBlueprintTests
    {
        [Fact]
        public void should_construct_from_attribute()
        {
            var blueprint = new AttributeBlueprint<Set>(x => x.Value);

            var result = blueprint.Construct(new ConstruktionContext(typeof(Foo).GetProperty("Bar")), Pipeline.Default);

            result.ShouldBe("Set");
        }

        public class Foo
        {
            [Set("Set")]
            public string Bar { get; set; }
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
