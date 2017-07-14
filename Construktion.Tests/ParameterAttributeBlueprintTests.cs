namespace Construktion.Tests
{
    using System;
    using System.Reflection;
    using Blueprints;
    using Shouldly;
    using Xunit;

    public class ParameterAttributeBlueprintTests
    {
        [Fact]
        public void should_match_paramater_with_attribute()
        {
            var blueprint = new SetBlueprint();
            var parameterInfo =
                typeof(ParameterAttributeBlueprintTests).GetMethod(nameof(TestMethod))
                    .GetParameters()[0];

            var matches = blueprint.Matches(new ConstruktionContext(parameterInfo));

            matches.ShouldBe(true);
        }

        [Fact]
        public void should_not_match_parameter_without_attribute()
        {
            var blueprint = new SetBlueprint();
            var parameterInfo =
                typeof(ParameterAttributeBlueprintTests).GetMethod(nameof(TestMethod))
                    .GetParameters()[1];


            var matches = blueprint.Matches(new ConstruktionContext(parameterInfo));

            matches.ShouldBe(false);
        }

        [Fact]
        public void should_construct_from_attribute()
        {
            var blueprint = new SetBlueprint();
            var parameterInfo =
                typeof(ParameterAttributeBlueprintTests).GetMethod(nameof(TestMethod))
                    .GetParameters()[0];


            var result = blueprint.Construct(new ConstruktionContext(parameterInfo), Default.Pipeline);

            result.ShouldBe("Set");
        }

        public void TestMethod([Set("Set")] string withAttribute, string withoutAttribute) { }

        public class SetBlueprint : ParameterAttributeBlueprint<Set>
        {
            public SetBlueprint() : base (x => x.Value)
            {
                
            }
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