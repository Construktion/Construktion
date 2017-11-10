using Construktion.Blueprints;
using Shouldly;
using Xunit;

namespace Construktion.Tests
{
    public class AbstractExitBlueprintTests
    {
        [Fact]
        public void should_apply_value_after_the_pipeline()
        {
            var construktion = new Construktion().With(x =>
            {
                x.OmitIds();
                x.AddExitBlueprint<FooExitBlueprint>();
            });

            var foo = construktion.Construct<Foo>();

            foo.Id.ShouldBe(10);
        }

        [Fact]
        public void should_not_alter_constructed_values()
        {
            var construktion = new Construktion().With(x =>
            {
                x.OmitIds();
                x.AddExitBlueprint<FooExitBlueprint>();
            });

            var foo = construktion.Construct<Foo>();

            foo.Name.ShouldNotBeNullOrWhiteSpace();
        }

        [Fact]
        public void should_work_for_interfaces()
        {
            var construktion = new Construktion().With(x =>
            {
                x.OmitIds();
                x.AddExitBlueprint<IFooExitBlueprint>();
            });

            var foo = construktion.Construct<Foo>();

            foo.Id.ShouldBe(10);
        }

        [Fact]
        public void should_override_matches()
        {
            var construktion = new Construktion().With(x =>
            {
                x.ConstructPropertyUsing(prop => prop.Name == "Name", () => "Ping");
                x.AddExitBlueprint<FooPingPongExitBlueprint>();
            });

            var foo = construktion.Construct<Foo>();

            foo.Name.ShouldBe("Pong");
        }

        public class FooExitBlueprint : AbstractExitBlueprint<Foo>
        {
            public override Foo Construct(Foo item, ConstruktionPipeline pipeline)
            {
                item.Id = 10;

                return item;
            }
        }

        public class IFooExitBlueprint : AbstractExitBlueprint<IFoo>
        {
            public override IFoo Construct(IFoo item, ConstruktionPipeline pipeline)
            {
                item.Id = 10;

                return item;
            }
        }

        public class FooPingPongExitBlueprint : AbstractExitBlueprint<Foo>
        {
            public override bool Matches(Foo item, ConstruktionContext context) => item.Name == "Ping";

            public override Foo Construct(Foo item, ConstruktionPipeline pipeline)
            {
                item.Name = "Pong";

                return item;
            }
        }

        public class Foo : IFoo
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public interface IFoo
        {
            int Id { get; set; }
        }
    }
}