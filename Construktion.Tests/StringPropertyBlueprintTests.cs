namespace Construktion.Tests
{
    using System.Reflection;
    using Blueprints;
    using Shouldly;
    using Xunit;

    public class StringPropertyBlueprintTests
    {
        [Fact]
        public void should_prefix_property_name()
        {
            var blueprint = new StringPropertyBlueprint();
            var pi = typeof(Foo).GetProperty(nameof(Foo.Name));
            var result = (string)blueprint.Construct(new ConstruktionContext(pi), Default.Pipeline);
            
            result.Substring(0, 5).ShouldBe("Name-");
        }

        public class Foo
        {
            public string Name { get; set; }
        }
    }
}