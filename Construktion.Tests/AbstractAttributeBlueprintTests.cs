namespace Construktion.Tests
{
    using System;
    using System.Reflection;
    using Acceptance;
    using global::Construktion.Blueprints;
    using Shouldly;
    using Xunit;

    public class AbstractAttributeBlueprintTests
    {
        [Fact]
        public void default_criteria_should_match_property_with_attribute()
        {
            var blueprint = new Setlueprint();
            var property = typeof(Foo).GetProperty("WithSet");
            var context = new BuildContext(property);

            var matches = blueprint.Matches(context);

            matches.ShouldBeTrue();
        }

        [Fact]
        public void property_without_attribute_should_not_match()
        {
            var blueprint = new Setlueprint();
            var property = typeof(Foo).GetProperty("WithoutSet");
            var context = new BuildContext(property);

            var matches = blueprint.Matches(context);

            matches.ShouldBeFalse();
        }

        public class Setlueprint : AbstractAttributeBlueprint<Set>
        {
            public override object Build(BuildContext context, ConstruktionPipeline pipeline)
            {
                throw new NotImplementedException();
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