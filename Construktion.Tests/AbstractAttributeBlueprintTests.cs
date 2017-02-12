namespace Construktion.Tests
{
    using System.Reflection;
    using Acceptance;
    using Blueprints;
    using global::Construktion.Blueprints;
    using Shouldly;
    using Xunit;

    public class AbstractAttributeBlueprintTests
    {
        [Fact]
        public void should_return_attribute_value()
        {
            var blueprint = new SetBlueprint();
            var property = typeof(Foo).GetProperty("WithSet");
            var context = new ConstruktionContext(property);

            var result = (string)blueprint.Build(context, Default.Pipeline);

            result.ShouldBe("Fubar");
        }
        [Fact]
        public void default_criteria_should_match_property_with_attribute()
        {
            var blueprint = new SetBlueprint();
            var property = typeof(Foo).GetProperty("WithSet");
            var context = new ConstruktionContext(property);

            var matches = blueprint.Matches(context);

            matches.ShouldBeTrue();
        }

        [Fact]
        public void property_without_attribute_should_not_match()
        {
            var blueprint = new SetBlueprint();
            var property = typeof(Foo).GetProperty("WithoutSet");
            var context = new ConstruktionContext(property);

            var matches = blueprint.Matches(context);

            matches.ShouldBeFalse();
        }

        public class SetBlueprint : AbstractAttributeBlueprint<Set>
        {
            public override object Build(ConstruktionContext context, ConstruktionPipeline pipeline)
            {
                return GetAttribute(context).Value;
            }
        }

        public class Foo
        {
            [Set("Fubar")]
            public string WithSet { get; set; }
            public string WithoutSet { get; set; }
        }
    }
}