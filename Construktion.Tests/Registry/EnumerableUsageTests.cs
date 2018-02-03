namespace Construktion.Tests.Registry
{
    using System.Linq;
    using Shouldly;

    public class EnumerableUsageTests
    {
        private readonly ConstruktionRegistry registry;
        private readonly Construktion construktion;

        public EnumerableUsageTests()
        {
            registry = new ConstruktionRegistry();
            construktion = new Construktion();
        }

        [Fact]
        public void default_count_should_be_3()
        {
            var ints = construktion.With(registry).ConstructMany<int>();

            ints.Count().ShouldBe(3);
        }

        [Fact]
        public void a_new_registry_should_not_overwrite_enumerable_count()
        {
            registry.EnumerableCount(1);

            var ints = construktion.With(registry)
                                   .With(new ConstruktionRegistry())
                                   .ConstructMany<int>();

            ints.Count().ShouldBe(1);
        }

        [Fact]
        public void a_registry_with_explicit_enumerable_count_should_overwrite_previous()
        {
            registry.EnumerableCount(1);

            var ints = construktion.With(registry)
                                   .With(new ConstruktionRegistry(x => x.EnumerableCount(2)))
                                   .ConstructMany<int>();

            ints.Count().ShouldBe(2);
        }
    }
}