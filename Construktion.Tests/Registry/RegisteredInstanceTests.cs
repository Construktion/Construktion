namespace Construktion.Tests.Registry
{
    using System;
    using Shouldly;
    using Xunit;

    public class RegisteredInstanceTests
    {
        private readonly ConstruktionRegistry _registry;
        private readonly Construktion _construktion;

        public RegisteredInstanceTests()
        {
            _registry = new ConstruktionRegistry();
            _construktion = new Construktion();
        }

        [Fact]
        public void should_register_instance_with_contract()
        {
            _registry.Register<IFoo, Foo>();

            var result = _construktion.With(_registry).Construct<IFoo>();

            result.ShouldBeOfType<Foo>();
        }

        [Fact]
        public void last_registered_instance_should_be_chosen()
        {
            _registry.Register<IFoo, Foo>();
            _registry.Register<IFoo, Foo2>();

            var result = _construktion.With(_registry).Construct<IFoo>();

            result.ShouldBeOfType<Foo2>();
        }

        [Fact]
        public void should_register_scoped_instance()
        {
            var foo = new Foo { FooId = -1};
            _registry.UseInstance<IFoo>(foo);

            var result = _construktion.With(_registry).Construct<IFoo>();

            var fooResult = result.ShouldBeOfType<Foo>();
            fooResult.FooId.ShouldBe(-1);
            fooResult.GetHashCode().ShouldBe(foo.GetHashCode());
        }

        [Fact]
        public void should_use_last_scoped_instance_registered()
        {
            var foo = new Foo();
            var foo2 = new Foo();
            _registry.UseInstance<IFoo>(foo);
            _registry.UseInstance<IFoo>(foo2);

            _construktion.With(_registry);

            var result = _construktion.Construct<IFoo>();

            result.GetHashCode().ShouldBe(foo2.GetHashCode());
        }

        [Fact]
        public void should_use_instance_across_graph()
        {
            var foo = new Foo();
            _registry.UseInstance<IFoo>(foo);

            _construktion.With(_registry);

            var result = _construktion.Construct<FooCollector>();

            result.CtorFoo.GetHashCode().ShouldBe(foo.GetHashCode());
            result.PropertyFoo.GetHashCode().ShouldBe(foo.GetHashCode());
        }

        [Fact]
        public void should_throw_when_interface_isnt_registered()
        {
            //_blueprintRegistry.Register<IFoo, Foo>();

            Should.Throw<Exception>
                (() => _construktion.With(_registry).Construct<IFoo>())
                .Message
                .ShouldContain("Cannot construct the interface IFoo.");
        }

        public interface IFoo { }

        public class Foo : IFoo
        {
            public int FooId { get; set; }
        }

        public class Foo2 : IFoo { }

        public class FooCollector
        {
            public IFoo CtorFoo { get; }
            public IFoo PropertyFoo { get; set; }

            public FooCollector(IFoo foo)
            {
                CtorFoo = foo;
            }
        }
    }
}
