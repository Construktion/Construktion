namespace Construktion.Tests.Registry
{
    using System;
    using Shouldly;

    public class RegisteredInstanceTests
    {
        private readonly ConstruktionRegistry registry;
        private readonly Construktion construktion;

        public RegisteredInstanceTests()
        {
            registry = new ConstruktionRegistry();
            construktion = new Construktion();
        }

        public void should_register_instance_with_contract()
        {
            registry.Register<IFoo, Foo>();

            var result = construktion.Apply(registry).Construct<IFoo>();

            result.ShouldBeOfType<Foo>();
        }

        public void last_registered_instance_should_be_chosen()
        {
            registry.Register<IFoo, Foo>();
            registry.Register<IFoo, Foo2>();

            var result = construktion.Apply(registry).Construct<IFoo>();

            result.ShouldBeOfType<Foo2>();
        }

        public void should_register_scoped_instance()
        {
            var foo = new Foo { FooId = -1 };
            registry.UseInstance<IFoo>(foo);

            var result = construktion.Apply(registry).Construct<IFoo>();

            var fooResult = result.ShouldBeOfType<Foo>();
            fooResult.FooId.ShouldBe(-1);
            fooResult.GetHashCode().ShouldBe(foo.GetHashCode());
        }

        public void should_use_last_scoped_instance_registered()
        {
            var foo = new Foo();
            var foo2 = new Foo();
            registry.UseInstance<IFoo>(foo);
            registry.UseInstance<IFoo>(foo2);

            construktion.Apply(registry);

            var result = construktion.Construct<IFoo>();

            result.GetHashCode().ShouldBe(foo2.GetHashCode());
        }

        public void should_use_instance_across_graph()
        {
            var foo = new Foo();
            registry.UseInstance<IFoo>(foo);

            construktion.Apply(registry);

            var result = construktion.Construct<FooCollector>();

            result.CtorFoo.GetHashCode().ShouldBe(foo.GetHashCode());
            result.PropertyFoo.GetHashCode().ShouldBe(foo.GetHashCode());
        }

        public void should_throw_when_interface_isnt_registered()
        {
            //_blueprintRegistry.Register<IFoo, Foo>();

            Exception<Exception>.ShouldBeThrownBy(() => construktion.Apply(registry).Construct<IFoo>())
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