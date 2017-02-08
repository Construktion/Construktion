namespace Construktion.Tests
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;
    using Builders;
    using Shouldly;
    using Xunit;

    public class AbstractAttributeBuilderTests
    {
        private readonly MaxLengthAttributeBuilder _maxLengthBuilder;

        public AbstractAttributeBuilderTests()
        {
            _maxLengthBuilder = new MaxLengthAttributeBuilder();
        }

        public class MaxLengthAttributeBuilder : AbstractAttributeBuilder<MaxLengthAttribute>
        {
            public override object Build(RequestContext context, ConstruktionPipeline pipeline)
            {
                throw new NotImplementedException();
            }
        }

        [Fact]
        public void Can_Build_When_Building_A_Property_With_Closed_Type_As_Attribute()
        {
            var context = new RequestContext(typeof(WithMaxLength).GetProperty(nameof(WithMaxLength.Property)));

            _maxLengthBuilder.CanBuild(context).ShouldBeTrue();
        }

        [Fact]
        public void Get_Attribute_Should_Resolve_To_Implementation()
        {
            var context = new RequestContext(typeof(WithMaxLength).GetProperty(nameof(WithMaxLength.Property)));

            var attr = _maxLengthBuilder.GetAttribute(context);

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
            var context =  new RequestContext(typeof(WithRequired).GetProperty(nameof(WithRequired.Property)));

            _maxLengthBuilder.CanBuild(context).ShouldBeFalse();
        }

        public class WithRequired
        {
            [Required]
            public string Property { get; set; }
        }
    }
}
