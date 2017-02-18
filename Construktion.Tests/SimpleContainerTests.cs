namespace Construktion.Tests
{
    using System;
    using Shouldly;
    using Xunit;

    public class SimpleContainerTests
    {
        [Fact]
        public void should_resolve_parameterless_ctor_instances()
        {
            var container = new SimpleContainer();
        
            var foo = container.GetInstance<Foo>();

            foo.ShouldNotBeNull();
        }

        [Fact]
        public void should_resolve_interface_with_implementation()
        {
            var container = new SimpleContainer();

            container.Register<IFoo, Foo>();
            
            container.GetInstance<IFoo>().ShouldBeOfType<Foo>();
        }
   
        [Fact]
        public void should_resolve_concrete_with_its_dependency()
        {
            var container = new SimpleContainer();
            container.Register<IFoo, Foo>();

            var bar = container.GetInstance<Bar>();

            bar.ShouldNotBeNull();
            bar.Foo.ShouldBeOfType<Foo>();
        }

        [Fact]
        public void should_resolve_a_deep_graph()
        {
            var container = new SimpleContainer();
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
            var container = new SimpleContainer();
            container.Register<IFoo, Foo>();
            container.Register<IBar, Bar>();

            var multipleDependencies = container.GetInstance<MultipleDependencies>();

            multipleDependencies.Bar.ShouldBeOfType<Bar>();
            multipleDependencies.Foo.ShouldBeOfType<Foo>();
        }

        [Fact]
        public void should_use_transient_instances()
        {
            var container = new SimpleContainer();
            container.Register<IFoo, Foo>();
            container.Register<IBar, Bar>();

            var multipleDependencies = container.GetInstance<MultipleDependencies>();

            multipleDependencies.Foo.GetHashCode().ShouldNotBe(multipleDependencies.Bar.Foo.GetHashCode());
        }

        [Fact]
        public void should_throw_and_tell_you_what_dependency_is_missing()
        {
            var container = new SimpleContainer();
            // container.Register<IFoo, Foo>();

            Should.Throw<Exception>(() =>
            {
                var bar = container.GetInstance<Bar>();

            }).Message.ShouldContain("Bar", "IFoo");
        }

        [Fact]
        public void should_resolve_greediest_ctor()
        {
            var container = new SimpleContainer();
            container.Register<IFoo, Foo>();
            container.Register<IBar, Bar>();

            var greedyCtor = container.GetInstance<GreedyCtor>();

            greedyCtor.Foo.ShouldBeOfType<Foo>();
            greedyCtor.Bar.ShouldBeOfType<Bar>();
        }

        [Fact]
        public void should_resolve_first_implementation_registered()
        {
            var container = new SimpleContainer();

            container.Register<IFoo, Foo>();
            container.Register<IFoo, Foo2>();

            container.GetInstance<IFoo>().ShouldBeOfType<Foo>();
        }

        public interface IFoo
        {
            
        }

        public class Foo : IFoo { }

        public class Foo2 : IFoo { }
         

        public interface IBar : IFoo
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

        public class GreedyCtor
        {
            public IBar Bar { get; }
            public IFoo Foo { get; }

            public GreedyCtor(IBar bar)
            {
                Bar = bar;
            }

            public GreedyCtor(IBar bar, IFoo foo)
            {
                Bar = bar;
                Foo = foo;
            }
        }
    }
}