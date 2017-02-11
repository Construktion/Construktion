namespace Construktion.Tests.Acceptance
{
    using System;
    using System.Linq;
    using global::Construktion.Blueprints;
    using Shouldly;
    using Xunit;

    public class AttributeTests
    {
        [Fact]
        public void builds_attribute_based_properties()
        {
           var construktion = new Construktion(new SetBlueprint());

            var result = construktion.Build<Foo>();

            result.Bar.ShouldBe("Value");
        }

        public class SetBlueprint : AbstractAttributeBlueprint<Set>
        {
            public override object Build(BuildContext context, ConstruktionPipeline pipeline)
            {
                var attribute = Attribute(context);

                return attribute.Value;
            }
        }

        [Fact]
        public void can_define_more_strict_rules()
        {
            var construktion = new Construktion(new BarStrictSetBlueprint());

            var result = construktion.Build<Foo>();

            result.Baz.ShouldNotBe("Value");
        }

        public class BarStrictSetBlueprint : AbstractAttributeBlueprint<Set>
        {
            protected override bool AlsoMustMatch(BuildContext context)
            {
                var propertyName = context.PropertyInfo.Single().Name;

                return propertyName == "Bar";
            }

            public override object Build(BuildContext context, ConstruktionPipeline pipeline)
            {
                var attribute = Attribute(context);

                return attribute.Value;
            }
        }

        public class Foo
        {
            [Set("Value")]
            public string Bar { get; set; }

            [Set("Value")]
            public string Baz { get; set; }
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
