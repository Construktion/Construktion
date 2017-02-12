namespace Construktion.Tests
{
    using System;
    using Shouldly;
    using Xunit;

    public class ContainerTests
    {
        [Fact]
        public void should_resolve_parameterless_ctor_instances()
        {
            var container = new ConstruktionContainer();
        
            var instance = container.GetInstance<Foo>();

            instance.ShouldNotBeNull();
        }

        [Fact]
        public void should_resolve_interface_with_implementation()
        {
            var container = new ConstruktionContainer();

            container.For<IFoo>().Use<Foo>();

            container.GetInstance<IFoo>().ShouldBeOfType<Foo>();
        }
   
        [Fact]
        public void should_resolve_instance_with_a_dependency()
        {
            var container = new ConstruktionContainer();
            container.For<IFoo>().Use<Foo>();

            var instance = container.GetInstance<Bar>();

            instance.ShouldNotBeNull();
            instance.FooD.ShouldBeOfType<Foo>();
        }

        [Fact]
        public void should_resolve_a_deep_graph()
        {
            var container = new ConstruktionContainer();
            container.For<IFoo>().Use<Foo>();
            container.For<IBar>().Use<Bar>();

            var instance = container.GetInstance<Baz>();

            instance.ShouldNotBeNull();
            instance.BarD.ShouldBeOfType<Bar>()
                .FooD.ShouldBeOfType<Foo>();
        }

        [Fact]
        public void should_resolve_all_dependencies()
        {
            var container = new ConstruktionContainer();
            container.For<IFoo>().Use<Foo>();
            container.For<IBar>().Use<Bar>();

            var instance = container.GetInstance<MultipleDependencies>();

            instance.Bar.ShouldBeOfType<Bar>();
            instance.Foo.ShouldBeOfType<Foo>();
        }

        [Fact]
        public void should_have_a_built_in_unit_of_work()
        {
            var container = new ConstruktionContainer();
            container.For<IFoo>().Use<Foo>();
            container.For<IBar>().Use<Bar>();

            var instance = container.GetInstance<MultipleDependencies>();

            instance.Foo.GetHashCode().ShouldBe(instance.Bar.FooD.GetHashCode());
        }

        public void should_throw_when_no_interface_implementation_is_registered()
        {
            var container = new ConstruktionContainer();

            Should.Throw<Exception>(() =>
            {
                var instance = container.GetInstance<IFoo>();
            })
            .Message.ShouldBe("No registered instance can be found for IFoo");
        }

        [Fact]
        public void should_tell_you_what_dependency_is_missing()
        {
            var container = new ConstruktionContainer();
            // container.For<IFoo>().Use<Foo>();

            Should.Throw<Exception>(() =>
            {
                var instance = container.GetInstance<Bar>();

            })
            .Message.ShouldContain("Ctor arg IFoo cannot be resolved.");
        }

        public interface IFoo
        {
            
        }

        public class Foo : IFoo { }

        public interface IBar
        {
             IFoo FooD { get; }
        }

        public class Bar : IBar
        {
            public IFoo FooD { get; }

            public Bar(IFoo foo)
            {
                FooD = foo;
            }
        }

        public class Baz
        {
            public IBar BarD { get; }

            public Baz(IBar bar)
            {
                BarD = bar;
            }
        }

      
        public class MultipleDependencies
        {
            public IBar Bar { get; }
            public IFoo Foo { get; }

            public MultipleDependencies(IBar bar, IFoo foo)
            {
                Bar = bar;
                Foo = foo;
            }
        }
    }
}