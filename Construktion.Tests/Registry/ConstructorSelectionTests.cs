namespace Construktion.Tests.Registry
{
    using Shouldly;
    using Xunit;

    public class ConstructorSelectionTests
    {
        private readonly ConstruktionRegistry registry;
        private readonly Construktion construktion;

        public ConstructorSelectionTests()
        {
            registry = new ConstruktionRegistry();
            construktion = new Construktion();
        }

        [Fact]
        public void should_resolve_modest_ctor_by_default()
        {
            var result = construktion.Construct<MultiCtor>();

            result.UsedModestCtor.ShouldBe(true);
        }

        [Fact]
        public void should_use_modest_ctor_when_opted_in()
        {
            registry.UseModestCtor();

            var result = construktion.With(registry).Construct<MultiCtor>();

            result.UsedModestCtor.ShouldBe(true);
        }

        [Fact]
        public void a_newregistry_without_a_ctor_strategy_should_not_overwrite_previous()
        {
            registry.UseModestCtor();

            construktion
                .With(registry)
                .With(new ConstruktionRegistry());

            var result = construktion.Construct<MultiCtor>();

            result.UsedModestCtor.ShouldBe(true);
        }

        [Fact]
        public void should_use_the_last_registered_ctor_strategy()
        {
            registry
                .UseGreedyCtor()
                .UseModestCtor();

            construktion.With(registry);

            var result = construktion.Construct<MultiCtor>();

            result.UsedModestCtor.ShouldBe(true);
        }

        public class MultiCtor
        {
            public bool UsedModestCtor { get; }
            public bool UsedGreedyCtor { get; }

            public MultiCtor(string one)
            {
                UsedModestCtor = true;
            }

            public MultiCtor(string one, string two)
            {
                UsedGreedyCtor = true;
            }
        }
    }
}
