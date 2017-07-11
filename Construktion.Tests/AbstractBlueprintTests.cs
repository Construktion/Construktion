namespace Construktion.Tests
{
    using Blueprints;
    using Shouldly;
    using Xunit;

    public class AbstractBlueprintTests
    {
        [Fact]
        public void should_construct_using_blueprint()
        {
            var context = new Construktion().AddBlueprint(new FooBlueprint());

            var result = context.Construct<Foo>();

            result.Name.ShouldBe("Name");
            result.Age.ShouldBe(10);
        }

        public class FooBlueprint : AbstractBlueprint<Foo>
        {
            public override object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
            {
                return new Foo
                {
                    Name = "Name",
                    Age = 10
                };
            }
        }

        public class Foo
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }
    }
}