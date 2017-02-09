namespace Construktion.Tests
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;
    using Blueprints;
    using Shouldly;
    using Xunit;

    public class AbstractAttributeBlueprintTests
    {
        private readonly MaxLengthAttributeBlueprint _maxLengthBlueprint;

        public AbstractAttributeBlueprintTests()
        {
            _maxLengthBlueprint = new MaxLengthAttributeBlueprint();
        }

        public class MaxLengthAttributeBlueprint : AbstractAttributeBlueprint<MaxLengthAttribute>
        {
            public override object Build(ConstruktionContext context, ConstruktionPipeline pipeline)
            {
                throw new NotImplementedException();
            }
        }

        [Fact]
        public void Can_Build_When_Building_A_Property_With_Closed_Type_As_Attribute()
        {
            var context = new ConstruktionContext(typeof(WithMaxLength).GetProperty(nameof(WithMaxLength.Property)));

            _maxLengthBlueprint.Matches(context).ShouldBeTrue();
        }

        [Fact]
        public void Get_Attribute_Should_Resolve_To_Implementation()
        {
            var context = new ConstruktionContext(typeof(WithMaxLength).GetProperty(nameof(WithMaxLength.Property)));

            var attr = _maxLengthBlueprint.GetAttribute(context);

            attr.Length.ShouldBe(12);
        }

        public class WithMaxLength
        {
            [MaxLength(12)]
            public string Property { get; set; }
        }

        [Fact]
        public void Will_Not_Build_Other_Attributes()
        {
            var context =  new ConstruktionContext(typeof(WithRequired).GetProperty(nameof(WithRequired.Property)));

            _maxLengthBlueprint.Matches(context).ShouldBeFalse();
        }

        public class WithRequired
        {
            [Required]
            public string Property { get; set; }
        }
    }
}
