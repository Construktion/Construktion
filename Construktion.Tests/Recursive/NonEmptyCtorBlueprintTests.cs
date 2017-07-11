namespace Construktion.Tests.Recursive
{
    using Blueprints.Recursive;
    using Shouldly;
    using Xunit;

    public class NonEmptyCtorBlueprintTests
    {
        [Fact]
        public void should_match_class_with_non_default_ctor()
        {
            var blueprint = new NonEmptyCtorBlueprint();

            var matches = blueprint.Matches(new ConstruktionContext(typeof(NonEmptyCtor)));

            matches.ShouldBe(true);
        }

        [Fact]
        public void should_not_match_class_with_default_ctor()
        {
            var blueprint = new NonEmptyCtorBlueprint();

            var matches = blueprint.Matches(new ConstruktionContext(typeof(EmptyCtor)));

            matches.ShouldBe(false);
        }

        [Fact]
        public void should_construct_ctor_arg_and_properties()
        {
            var blueprint = new NonEmptyCtorBlueprint();
            var pipeline = new DefaultConstruktionPipeline(Default.Blueprints.Replace(typeof(NonEmptyCtorBlueprint), blueprint));

            var bar = (NonEmptyCtor)blueprint.Construct(new ConstruktionContext(typeof(NonEmptyCtor)), pipeline);
            
            bar.Name.ShouldNotBeNullOrWhiteSpace();
            bar.Age.ShouldNotBe(0);

            bar.Foo.ShouldNotBeNull();
            bar.Foo.Name.ShouldNotBeNullOrWhiteSpace();
            bar.Foo.Age.ShouldNotBe(0);
        }

        public class EmptyCtor { }      

        public class NonEmptyCtor
        {
            public Foo Foo { get; private set; } 

            public string Name { get; set; }
            public int Age { get; set; }

            public NonEmptyCtor(Foo foo)
            {
                Foo = foo;
            }
        }

        public class Foo
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }
    }
}