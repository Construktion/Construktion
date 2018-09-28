namespace Construktion.Tests.Registry
{
    using Shouldly;

    public class InjectTests
    {
        public void should_inject_instance()
        {
            var foo = new Foo();
            var construktion = new Construktion().Inject(foo);

            var result = construktion.Construct<FooHolder>();

            result.Foo
                  .GetHashCode()
                  .ShouldBe(foo.GetHashCode());
        }

        public void should_inject_instance_with_explicit_type()
        {
            var foo = new Foo() { Name = "Foo" };
            var fooHolder = new FooHolder(foo);

            var construktion = new Construktion().Inject(typeof(FooHolder), fooHolder);

            var result = construktion.Construct<FooHolder>();

            result.Foo.Name.ShouldBe("Foo");
            result.ShouldBeOfType<FooHolder>();
            result.GetHashCode().ShouldBe(fooHolder.GetHashCode());
        }

        public void should_always_use_the_same_instance()
        {
            var foo = new Foo();
            var construktion = new Construktion().Inject(foo);

            var result = construktion.Construct<FooHolder>();
            var result2 = construktion.Construct<FooHolder>();

            result.Foo
                  .GetHashCode()
                  .ShouldBe(foo.GetHashCode());

            result2.Foo
                   .GetHashCode()
                   .ShouldBe(foo.GetHashCode());
        }

        public void should_inject_from_within_pipeline()
        {
            var construktion = new Construktion().Apply(new FooBlueprint());

            var foo = construktion.Construct<Foo>();
            var result = construktion.Construct<FooHolder>();

            result.Foo
                  .GetHashCode()
                  .ShouldBe(foo.GetHashCode());
        }

        public void should_always_use_the_same_instance_when_injected_from_pipeline()
        {
            var construktion = new Construktion().Apply(new FooBlueprint());

            var foo = construktion.Construct<Foo>();
            var result = construktion.Construct<FooHolder>();
            var result2 = construktion.Construct<FooHolder>();

            foo.GetHashCode().ShouldBe(result.Foo.GetHashCode());

            result.Foo
                        .GetHashCode()
                        .ShouldBe(result2.Foo.GetHashCode());
        }

        public void injected_instance_should_be_scoped_to_construktion_instance()
        {
            var foo = new Foo();

            var construktion = new Construktion().Inject(foo);

            var injectHolder = new Construktion().Construct<FooHolder>();

            injectHolder.Foo
                        .GetHashCode()
                        .ShouldNotBe(foo.GetHashCode());
        }

        public class FooBlueprint : Blueprint
        {
            public bool Matches(ConstruktionContext context) => context.RequestType == typeof(Foo);

            public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
            {
                var inject = new Foo();

                pipeline.Inject(context.RequestType, inject);

                return inject;
            }
        }

        public class Foo
        {
            public string Name { get; set; }
        }

        public class FooHolder
        {
            public Foo Foo { get; }

            public FooHolder(Foo foo)
            {
                Foo = foo;
            }
        }
    }
}