namespace Construktion.Tests.Registry
{
    using Shouldly;
    using Xunit;

    public class PropertySelectionTests
    {
        private readonly ConstruktionRegistry _registry;
        private readonly Construktion _construktion;

        public PropertySelectionTests()
        {
            _registry = new ConstruktionRegistry();
            _construktion = new Construktion();
        }

        [Fact]
        public void should_not_construct_private_setters_by_default()
        {
            _construktion.With(_registry);

            var foo = _construktion.Construct<Foo>();

            foo.PrivateSetter.ShouldBeNullOrWhiteSpace();
        }

        [Fact]
        public void should_opt_in_to_constructing_properties_with_private_setter()
        {
            _construktion.With(x => x.ConstructPrivateSetters());

            var foo = _construktion.Construct<Foo>();

            foo.PrivateSetter.ShouldNotBeNullOrWhiteSpace();
        }

        [Fact]
        public void should_overwrite_previous_property_selector()
        {
            _registry
                .ConstructPrivateSetters()
                .OmitPrivateSetters();

            _construktion.With(_registry);

            var foo = _construktion.Construct<Foo>();

            foo.PrivateSetter.ShouldBeNullOrWhiteSpace();
        }

        public class Foo
        {
            public string PrivateSetter { get; private set; }
        }
    }
}
