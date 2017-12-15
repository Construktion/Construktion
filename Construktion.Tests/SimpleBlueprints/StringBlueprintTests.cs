namespace Construktion.Tests.SimpleBlueprints
{
    using Blueprints.Simple;
    using Internal;
    using Shouldly;
    using Xunit;

    public class StringBlueprintTests
    {
        [Fact]
        public void should_construct()
        {
            var blueprint = new StringBlueprint();

            var result = (string)blueprint.Construct(new ConstruktionContext(typeof(string)),
                new DefaultConstruktionPipeline());

            result.ShouldStartWith("String-");
        }

        [Fact]
        public void should_prefix_property_with_name()
        {
            var blueprint = new StringBlueprint();
            var pi = typeof(Foo).GetProperty(nameof(Foo.Name));
            var result = (string)blueprint.Construct(new ConstruktionContext(pi), new DefaultConstruktionPipeline());

            result.ShouldStartWith("Name-");
        }

        public class Foo
        {
            public string Name { get; set; }
        }
    }
}