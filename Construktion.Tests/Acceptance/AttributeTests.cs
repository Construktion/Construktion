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
        public void should_set_value_from_attribute()
        {
            var construktion = new Construktion(new SetBlueprint());

            var result = construktion.Build<Foo>();

            result.Bar.ShouldBe("Set");
            result.Baz.ShouldBe("Set");
        }

        public class SetBlueprint : AbstractAttributeBlueprint<Set>
        {
            public override object Build(ConstruktionContext context, ConstruktionPipeline pipeline)
            {
                var attribute = GetAttribute(context);

                return attribute.Value;
            }
        }

        [Fact]
        public void should_only_build_bar_property()
        {
            var construktion = new Construktion(new BarStrictSetBlueprint());

            var result = construktion.Build<Foo>();

            result.Bar.ShouldBe("Set");
            result.Baz.ShouldNotBe("Set");
        }

        public class BarStrictSetBlueprint : AbstractAttributeBlueprint<Set>
        {
            protected override bool AlsoMustMatch(ConstruktionContext context)
            {
                var propertyName = context.PropertyInfo.Single().Name;

                return propertyName == "Bar";
            }

            public override object Build(ConstruktionContext context, ConstruktionPipeline pipeline)
            {
                var attribute = GetAttribute(context);

                return attribute.Value;
            }
        }

        public class Foo
        {
            [Set("Set")]
            public string Bar { get; set; }

            [Set("Set")]
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
