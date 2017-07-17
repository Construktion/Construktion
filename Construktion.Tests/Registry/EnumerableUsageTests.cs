namespace Construktion.Tests.Registry
{
    using System.Linq;
    using Shouldly;
    using Xunit;

    public class EnumerableUsageTests
    {
        private readonly ConstruktionRegistry _registry;
        private readonly Construktion _construktion;

        public EnumerableUsageTests()
        {
            _registry = new ConstruktionRegistry();
            _construktion = new Construktion();
        }

        [Fact]
        public void default_count_should_be_3()
        {
            var ints = _construktion.With(_registry).ConstructMany<int>();

            ints.Count().ShouldBe(3);
        }

        [Fact]
        public void a_new_registry_should_not_overwrite_enumerable_count()
        {
            _registry.EnumerableCount(1);
            _registry.AddRegistry(new ConstruktionRegistry());

            var ints = _construktion.With(_registry).ConstructMany<int>();

            ints.Count().ShouldBe(1);
        }

        [Fact]
        public void a_registry_with_explicit_enumerable_count_should_overwrite_previous()
        {
            _registry.EnumerableCount(1);
            _registry.AddRegistry(new ConstruktionRegistry(x => x.EnumerableCount(2)));

            var ints = _construktion.With(_registry).ConstructMany<int>();

            ints.Count().ShouldBe(2);
        }
    }
}
