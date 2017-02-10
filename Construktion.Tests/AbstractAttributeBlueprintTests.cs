namespace Construktion.Tests
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using global::Construktion.Blueprints;
    using Shouldly;
    using Xunit;

    public class AbstractAttributeBlueprintTests
    {
        private readonly MaxLengthAttributeBlueprint _maxLengthBlueprint;
        private readonly BarMaxLengthAttributeBlueprint _barsMaxLengthBlueprint;

        public AbstractAttributeBlueprintTests()
        {
            _maxLengthBlueprint = new MaxLengthAttributeBlueprint();
            _barsMaxLengthBlueprint = new BarMaxLengthAttributeBlueprint();
        }

        [Fact]
        public void matches_when_property_info_has_attribute_of_t()
        {
            var pi = typeof(Foo).GetProperty(nameof(Foo.MaxLengthProperty));
            var context = new ConstruktionContext(pi);

            _maxLengthBlueprint.Matches(context).ShouldBeTrue();
        }

        [Fact]
        public void does_not_match_when_property_info_does_not_have_attribute_of_t()
        {
            var pi = typeof(Foo).GetProperty(nameof(Foo.RequiredProperty));
            var context = new ConstruktionContext(pi);

            _maxLengthBlueprint.Matches(context).ShouldBeFalse();
        }

        [Fact]
        public void should_match_if_additional_criteria_is_met()
        {
            var pi = typeof(Bar).GetProperty(nameof(Bar.MaxLengthProperty));
            var context = new ConstruktionContext(pi);

            _barsMaxLengthBlueprint.Matches(context).ShouldBeTrue();
        }

        [Fact]
        public void should_not_match_if_additional_criteria_is_not_met()
        {
            var pi = typeof(Foo).GetProperty(nameof(Foo.MaxLengthProperty));
            var context = new ConstruktionContext(pi);

            _barsMaxLengthBlueprint.Matches(context).ShouldBeFalse();
        }

        [Fact]
        public void base_additional_criteria_always_returns_true()
        {
            var attr = new MaxLengthAttributeBlueprint();
            attr.BaseAlsoMustMatch().ShouldBeTrue();
        }


        public class Foo
        {
            [MaxLength(12)]
            public string MaxLengthProperty { get; set; }

            [Required]
            public string RequiredProperty { get; set; }
        }

        public class Bar
        {
            [MaxLength(12)]
            public string MaxLengthProperty { get; set; }
        }

        public class MaxLengthAttributeBlueprint : AbstractAttributeBlueprint<MaxLengthAttribute>
        {
            public bool BaseAlsoMustMatch()
            {
                return base.AlsoMustMatch(new ConstruktionContext(typeof(Dummy)));
            }

            public override object Build(ConstruktionContext context, ConstruktionPipeline pipeline)
            {
                throw new NotImplementedException();
            }
        }

        public class BarMaxLengthAttributeBlueprint : AbstractAttributeBlueprint<MaxLengthAttribute>
        {
            protected override bool AlsoMustMatch(ConstruktionContext context)
            {
                return context.ParentClass.HasValue() && context.ParentClass.Single() == typeof(Bar);
            }

            public override object Build(ConstruktionContext context, ConstruktionPipeline pipeline)
            {
                throw new NotImplementedException();
            }
        }

        public class Dummy
        {

        }
    }
}
