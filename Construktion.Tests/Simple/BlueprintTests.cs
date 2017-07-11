namespace Construktion.Tests.Simple
{
    using Blueprints.Simple;
    using Shouldly;
    using Xunit;

    public class BlueprintTests
    {
        [Fact]
        public void should_match_type_t()
        {
            var blueprint = new Blueprint<Foo>();

            var matches = blueprint.Matches(new ConstruktionContext(typeof(Foo)));

            matches.ShouldBe(true);
        }

        [Fact]
        public void should_construct_t()
        {
            var blueprint = new Blueprint<Foo>();

            var result = (Foo)blueprint.Construct(new ConstruktionContext(typeof(Foo)), Default.Pipeline);

            result.Name.ShouldNotBeNullOrWhiteSpace();
            result.Age.ShouldNotBe(0);
        }

        public class Foo
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }
    }
}