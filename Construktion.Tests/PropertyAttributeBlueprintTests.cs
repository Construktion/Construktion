namespace Construktion.Tests
{
    using System;
    using System.Reflection;
    using Blueprints;
    using Shouldly;
    using Xunit;

    public class PropertyAttributeBlueprintTests
    {
        [Fact]
        public void should_match_property_with_attribute()
        {
            var blueprint = new SetBlueprint();
            var property = typeof(Foo).GetProperty("WithAttribute");
            var context = new ConstruktionContext(property);

            var matches = blueprint.Matches(context);

            matches.ShouldBe(true);
        }

        [Fact]
        public void should_not_match_property_without_attribute()
        {
            var blueprint = new SetBlueprint();
            var property = typeof(Foo).GetProperty("WithoutAttribute");
            var context = new ConstruktionContext(property);

            var matches = blueprint.Matches(context);

            matches.ShouldBe(false);
        }

        [Fact]
        public void should_construct_from_attribute()
        {
            var blueprint = new SetBlueprint();
            var property = typeof(Foo).GetProperty("WithAttribute");
            var context = new ConstruktionContext(property);

            var result = (string)blueprint.Construct(context, new DefaultConstruktionPipeline());

            result.ShouldBe("Set");
        }

        public class SetBlueprint : PropertyAttributeBlueprint<Set>
        {
            public SetBlueprint() : base (x => x.Value)
            {
                
            }
        }

        public class Foo
        {
            [Set("Set")]
            public string WithAttribute { get; set; }
            public string WithoutAttribute { get; set; }
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