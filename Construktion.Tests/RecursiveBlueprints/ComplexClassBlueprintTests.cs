﻿namespace Construktion.Tests.RecursiveBlueprints
{
    using Blueprints.Recursive;
    using Internal;
    using Shouldly;

    public class ComplexClassBlueprintTests
    {
        public void should_match_class_with_non_default_ctor()
        {
            var blueprint = new ComplexClassBlueprint();

            var matches = blueprint.Matches(new ConstruktionContext(typeof(NonEmptyCtor)));

            matches.ShouldBe(true);
        }

        public void should_match_class_with_default_ctor()
        {
            var blueprint = new ComplexClassBlueprint();

            var matches = blueprint.Matches(new ConstruktionContext(typeof(EmptyCtor)));

            matches.ShouldBe(true);
        }

        public void should_construct_ctor_arg_and_properties()
        {
            var blueprint = new ComplexClassBlueprint();

            var bar = (NonEmptyCtor)blueprint.Construct(new ConstruktionContext(typeof(NonEmptyCtor)),
                new DefaultConstruktionPipeline());

            bar.Name.ShouldNotBeNullOrWhiteSpace();
            bar.Age.ShouldNotBe(0);

            bar.Foo.ShouldNotBeNull();
            bar.Foo.Name.ShouldNotBeNullOrWhiteSpace();
            bar.Foo.Age.ShouldNotBe(0);
        }

        public void resolved_ctor_args_should_be_different_objects()
        {
            var result1 = new Construktion().Construct<NonEmptyCtor>();
            var result2 = new Construktion().Construct<NonEmptyCtor>();

            result1.Foo.ShouldNotBeNull();
            result2.Foo.ShouldNotBeNull();
            result1.Foo.GetHashCode().ShouldNotBe(result2.Foo.GetHashCode());
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