namespace Construktion.Tests.Registry
{
    using Shouldly;

    public class PropertySelectionTests
    {
        private readonly ConstruktionRegistry registry;
        private readonly Construktion construktion;

        public PropertySelectionTests()
        {
            registry = new ConstruktionRegistry();
            construktion = new Construktion();
        }

        public void should_not_construct_private_setters_by_default()
        {
            construktion.With(registry);

            var foo = construktion.Construct<Foo>();

            foo.PrivateSetter.ShouldBeNullOrWhiteSpace();
        }

        public void should_opt_in_to_constructing_properties_with_private_setter()
        {
            construktion.With(x => x.ConstructPrivateSetters());

            var foo = construktion.Construct<Foo>();

            foo.PrivateSetter.ShouldNotBeNullOrWhiteSpace();
        }

        public void should_overwrite_previous_property_selector()
        {
            registry
                .ConstructPrivateSetters()
                .OmitPrivateSetters();

            construktion.With(registry);

            var foo = construktion.Construct<Foo>();

            foo.PrivateSetter.ShouldBeNullOrWhiteSpace();
        }

        public void a_new_registry_without_a_property_strategy_should_not_overwrite_previous()
        {
            registry.ConstructPrivateSetters();

            construktion
                .With(registry)
                .With(new ConstruktionRegistry());

            var result = construktion.Construct<Foo>();

            result.PrivateSetter.ShouldNotBeNullOrWhiteSpace();
        }

        public class Foo
        {
            public string PrivateSetter { get; private set; }
        }
    }
}