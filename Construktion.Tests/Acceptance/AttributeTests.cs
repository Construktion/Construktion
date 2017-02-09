namespace Construktion.Tests.Acceptance
{
    using System;
    using Blueprints;
    using Shouldly;
    using Xunit;

    public class AttributeTests
    {
        [Fact]
        public void builds_attribute_based_properties()
        {
           var construktion = new Construktion(new SetAttributeBlueprint());

            var result = construktion.Build<Foo>();

            result.Bar.ShouldBe(5);
        }

        public class SetAttributeBlueprint : AbstractAttributeBlueprint<SetAttribute>
        {
            public override object Build(ConstruktionContext context, ConstruktionPipeline pipeline)
            {
                var attribute = GetAttribute(context);

                return attribute.Value;
            }
        }

        public class SetAttribute : Attribute
        {
            public int Value { get; }

            public SetAttribute(int value)
            {
                Value = value;
            }
        }

        public class Foo
        {
            [Set(5)]
            public int Bar { get; set; }
        }
    }
}
