namespace Construktion.Tests.Simple
{
    using System;
    using System.Reflection;
    using Blueprints.Simple;
    using Shouldly;
    using Xunit;

    public class CustomPropertyValueBlueprintTests
    {
        [Fact]
        public void should_match_defined_convention()
        { 
            var blueprint = new CustomPropertyValueBlueprint(x => x.Name.Equals("HireDate"), () => DateTime.MaxValue);

            var matches = blueprint.Matches(new ConstruktionContext(typeof(Foo).GetProperty(nameof(Foo.HireDate))));

            matches.ShouldBe(true);
        }

        [Fact]
        public void should_construct_from_value()
        {
            var blueprint = new CustomPropertyValueBlueprint(x => x.Name.Equals("HireDate"), () => DateTime.MaxValue);

            var result = blueprint.Construct(new ConstruktionContext(typeof(Foo).GetProperty(nameof(Foo.HireDate))),
                new DefaultConstruktionPipeline());

            result.ShouldBeOfType<DateTime>().ShouldBe(DateTime.MaxValue);
        }

        public class Foo
        {
            public DateTime HireDate { get; set; }
        }
    }
}
