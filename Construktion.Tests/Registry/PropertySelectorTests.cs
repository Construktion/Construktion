using Shouldly;
using Xunit;

namespace Construktion.Tests.Registry
{
    public class PropertySelectionTests
    {
        private readonly ConstruktionRegistry registry;
        private readonly Construktion construktion;

        public PropertySelectionTests()
        {
            registry = new ConstruktionRegistry();
            construktion = new Construktion();
        }

        [Fact]
        public void should_not_construct_private_setters_by_default()
        {
            construktion.With(registry);

            var foo = construktion.Construct<Foo>();

            foo.PrivateSetter.ShouldBeNullOrWhiteSpace();
        }

        [Fact]
        public void should_opt_in_to_constructing_properties_with_private_setter()
        {
            construktion.With(x => x.ConstructPrivateSetters());

            var foo = construktion.Construct<Foo>();

            foo.PrivateSetter.ShouldNotBeNullOrWhiteSpace();
        }

        [Fact]
        public void should_overwrite_previous_property_selector()
        {
            registry
                .ConstructPrivateSetters()
                .OmitPrivateSetters();

            construktion.With(registry);

            var foo = construktion.Construct<Foo>();

            foo.PrivateSetter.ShouldBeNullOrWhiteSpace();
        }

        public class Foo
        {
            public string PrivateSetter { get; private set; }
        }
    }
}
