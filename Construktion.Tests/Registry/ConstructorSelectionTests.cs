namespace Construktion.Tests.Registry
{
    using Shouldly;
    using Xunit;

    public class ConstructorSelectionTests
    {
        private readonly ConstruktionRegistry _registry;
        private readonly Construktion _construktion;

        public ConstructorSelectionTests()
        {
            _registry = new ConstruktionRegistry();
            _construktion = new Construktion();
        }

        [Fact]
        public void should_resolve_greediest_ctor_by_default()
        {
            var result = _construktion.Construct<MultiCtor>();

            result.UsedGreedyCtor.ShouldBe(true);
        }

        [Fact]
        public void should_use_modest_ctor_when_opted_in()
        {
            _registry.UseModestCtor();

            var result = _construktion.With(_registry).Construct<MultiCtor>();

            result.UsedModestCtor.ShouldBe(true);
        }

        [Fact]
        public void a_new_registry_without_a_ctor_strategy_should_not_overwrite_previous()
        {
            _registry.UseModestCtor();

            _construktion
                .With(_registry)
                .With(new ConstruktionRegistry());

            var result = _construktion.Construct<MultiCtor>();

            result.UsedModestCtor.ShouldBe(true);
        }

        [Fact]
        public void should_use_the_last_registered_ctor_strategy()
        {
            _registry
                .UseGreedyCtor()
                .UseModestCtor();

            _construktion.With(_registry);

            var result = _construktion.Construct<MultiCtor>();

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
