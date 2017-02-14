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
        
            var foo = container.GetInstance<Foo>();

            foo.ShouldNotBeNull();
        }

        [Fact]
        public void should_resolve_interface_with_implementation()
        {
            var container = new ConstruktionContainer();

            container.Register<IFoo, Foo>();
            
            container.GetInstance<IFoo>().ShouldBeOfType<Foo>();
        }
   
        [Fact]
        public void should_resolve_instance_with_a_dependency()
        {
            var container = new ConstruktionContainer();
            container.Register<IFoo, Foo>();

            var bar = container.GetInstance<Bar>();

            bar.ShouldNotBeNull();
            bar.Foo.ShouldBeOfType<Foo>();
        }

        [Fact]
        public void should_resolve_a_deep_graph()
        {
            var container = new ConstruktionContainer();
            container.Register<IFoo, Foo>();
            container.Register<IBar, Bar>();

            var baz = container.GetInstance<Baz>();

            baz.ShouldNotBeNull();
            baz.Bar
                .ShouldBeOfType<Bar>()
                .Foo
                .ShouldBeOfType<Foo>();
        }

        [Fact]
        public void should_resolve_all_dependencies()
        {
            var container = new ConstruktionContainer();
            container.Register<IFoo, Foo>();
            container.Register<IBar, Bar>();

            var multipleDependencies = container.GetInstance<MultipleDependencies>();

            multipleDependencies.Bar.ShouldBeOfType<Bar>();
            multipleDependencies.Foo.ShouldBeOfType<Foo>();
        }

        [Fact]
        public void should_use_transient_instances()
        {
            var container = new ConstruktionContainer();
            container.Register<IFoo, Foo>();
            container.Register<IBar, Bar>();

            var multipleDependencies = container.GetInstance<MultipleDependencies>();

            multipleDependencies.Foo.GetHashCode().ShouldNotBe(multipleDependencies.Bar.Foo.GetHashCode());
        }

        public void should_throw_when_no_interface_implementation_is_registered()
        {
            var container = new ConstruktionContainer();

            Should.Throw<Exception>(() =>
            {
                var foo = container.GetInstance<IFoo>();
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
                var bar = container.GetInstance<Bar>();

            })
            .Message.ShouldContain("No registered instance can be found for IFoo");
        }

        [Fact]
        public void should_resolve_first_implementation_registered()
        {
            var container = new ConstruktionContainer();

            container.Register<IFoo, Foo>();
            container.Register<IFoo, Foo2>();

            container.GetInstance<IFoo>().ShouldBeOfType<Foo>();
        }

        public interface IFoo
        {
            
        }

        public class Foo : IFoo { }

        public class Foo2 : IFoo { }
         

        public interface IBar
        {
             IFoo Foo { get; }
        }

        public class Bar : IBar
        {
            public IFoo Foo { get; }

            public Bar(IFoo foo)
            {
                Foo = foo;
            }
        }

        public class Baz
        {
            public IBar Bar { get; }

            public Baz(IBar bar)
            {
                Bar = bar;
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