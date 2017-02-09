namespace Construktion.Tests.Acceptance
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Builders;
    using Shouldly;
    using Xunit;

    public class AttributeBasedTests
    {
        [Fact]
        public void Should_Build_Attribute_Based_Properties()
        {
           var construktion = new Construktion(new List<Builder>{ new RangeAttributeBuilder() });

            var result = construktion.Build<AgeHasRange>();

            result.Age.ShouldBe(5);
        }
       
        public class RangeAttributeBuilder : AbstractAttributeBuilder<RangeAttribute>
        {
            private readonly Random _rnd = new Random();

            public override object Build(ConstruktionContext context, ConstruktionPipeline pipeline)
            {
                var attribute = GetAttribute(context);

                return _rnd.Next((int)attribute.Minimum, (int) attribute.Maximum + 1);
            }
        }

        public class AgeHasRange
        {
            [Range(5, 5)]
            public int Age { get; set; }
        }
    }
}
