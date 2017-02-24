namespace Construktion.Tests.Recursive
{
    using System;
    using System.Collections.Generic;
    using Blueprints.Recursive;
    using Shouldly;
    using Xunit;

    public class NonEmptyCtorTests
    {
        [Fact]
        public void should_match_class_with_non_default_ctor_and_not_match_class_with_default_ctor()
        {
            var blueprint = new NonEmptyCtorBlueprint(new Dictionary<Type, Type>());

            var matchesNonEmptyCtor = blueprint.Matches(new ConstruktionContext(typeof(Bar)));
            var matchesEmptyCtor = blueprint.Matches(new ConstruktionContext(typeof(EmptyCtor)));

            matchesNonEmptyCtor.ShouldBe(true);
            matchesEmptyCtor.ShouldBe(false);
        }

        [Fact]
        public void should_construct_graph_and_auto_properties()
        {
            var registry = new BlueprintRegistry();
            registry.Register<IFoo,Foo>();
            registry.Register<IBar, Bar>();
            var construktion = new Construktion().WithRegistry(registry);

            var bar = construktion.Construct<IBar>();

            bar.Name.ShouldNotBeNullOrWhiteSpace();
            bar.Age.ShouldNotBe(0);
            bar.Foo.Name.ShouldNotBeNullOrWhiteSpace();
            bar.Foo.Age.ShouldNotBe(0);
        }

        public interface IPerson
        {
            string Name { get; }
            int Age { get; }
        }

        public interface IFoo : IPerson
        {

        }

        public interface IBar : IFoo
        {
            IFoo Foo { get; }
        }

        public class Foo : IFoo
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        public class EmptyCtor { }

        public class Bar : IBar
        {
            public IFoo Foo { get; private set; } 

            public string Name { get; set; }
            public int Age { get; set; }

            public Bar(IFoo foo)
            {
                Foo = foo;
            }
        }
    }
}