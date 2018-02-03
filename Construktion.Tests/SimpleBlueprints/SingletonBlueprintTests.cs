namespace Construktion.Tests.SimpleBlueprints
{
    using Blueprints.Simple;
    using Internal;
    using Shouldly;

    public class SingletonBlueprintTests
    {
        public void should_match_specific_objects()
        {
            var foo = new Foo();
            var blueprint = new SingletonBlueprint(typeof(IFoo), foo);

            var matchesFoo = blueprint.Matches(new ConstruktionContext(typeof(IFoo)));
            var matchesString = blueprint.Matches(new ConstruktionContext(typeof(string)));

            matchesFoo.ShouldBe(true);
            matchesString.ShouldBe(false);
        }

        public void should_return_the_same_object()
        {
            var foo = new Foo { Name = "Name", Age = 10 };
            var blueprint = new SingletonBlueprint(typeof(IFoo), foo);

            var result = blueprint.Construct(new ConstruktionContext(typeof(IFoo)), new DefaultConstruktionPipeline());

            var fooResult = result.ShouldBeOfType<Foo>();
            fooResult.Name.ShouldBe("Name");
            fooResult.Age.ShouldBe(10);
            fooResult.GetHashCode().ShouldBe(foo.GetHashCode());
        }

        public interface IFoo { }

        public class Foo : IFoo
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }
    }
}