namespace Construktion.Tests
{
    using Shouldly;

    public class AbstractBlueprintTests
    {
        [Fact]
        public void should_match_t()
        {
            var blueprint = new FooBlueprint();

            blueprint.Matches(new ConstruktionContext(typeof(Foo))).ShouldBe(true);
        }

        [Fact]
        public void should_construct()
        {
            var context = new Construktion().With(new FooBlueprint());

            var result = context.Construct<Foo>();

            result.Name.ShouldBe("Name");
            result.Age.ShouldBe(10);
        }

        public class FooBlueprint : AbstractBlueprint<Foo>
        {
            public override Foo Construct(ConstruktionContext context, ConstruktionPipeline pipeline) => new Foo
            {
                Name = "Name",
                Age = 10
            };
        }

        public class Foo
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }
    }
}