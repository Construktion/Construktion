namespace Construktion.Tests.Registry
{
    using Shouldly;

    public class ConstructorSelectionTests
    {
        private readonly ConstruktionRegistry registry;
        private readonly Construktion construktion;

        public ConstructorSelectionTests()
        {
            registry = new ConstruktionRegistry();
            construktion = new Construktion();
        }

        public void should_resolve_modest_ctor_by_default()
        {
            var result = construktion.Construct<MultiCtor>();

            result.UsedModestCtor.ShouldBe(true);
        }

        public void should_use_greedy_ctor_when_opted_in()
        {
            registry.UseGreedyCtor();

            var result = construktion.With(registry).Construct<MultiCtor>();

            result.UsedGreedyCtor.ShouldBe(true);
        }

        public void a_new_registry_without_a_ctor_strategy_should_not_overwrite_previous()
        {
            registry.UseGreedyCtor();

            construktion
                .With(registry)
                .With(new ConstruktionRegistry());

            var result = construktion.Construct<MultiCtor>();

            result.UsedGreedyCtor.ShouldBe(true);
        }

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