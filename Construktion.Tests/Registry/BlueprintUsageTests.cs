namespace Construktion.Tests.Registry
{
    using Blueprints;
    using Shouldly;
    using Xunit;

    public class BlueprintUsageTests
    {
        private readonly ConstruktionRegistry _registry;
        private readonly Construktion _construktion;

        public BlueprintUsageTests()
        {
            _registry = new ConstruktionRegistry();
            _construktion = new Construktion();
        }

        [Fact]
        public void should_register_a_custom_blueprint()
        {
            _registry.AddBlueprint(new StringOneBlueprint());

            var result = _construktion.With(_registry).Construct<string>();

            result.ShouldBe("StringOne");
        }

        [Fact]
        public void should_register_via_generic_parameter()
        {
            _registry.AddBlueprint<StringOneBlueprint>();

            var result = _construktion.With(_registry).Construct<string>();

            result.ShouldBe("StringOne");
        }

        [Fact]
        public void blue_prints_registered_first_are_chosen_first()
        {
            _registry.AddBlueprint(new StringTwoBlueprint());
            _registry.AddBlueprint(new StringOneBlueprint());

            var result = _construktion.With(_registry).Construct<string>();

            result.ShouldBe("StringTwo");
        }

        [Fact]
        public void registries_registered_first_should_have_their_blueprints_used_first()
        {
            _construktion
                .With(new StringTwoRegistry())
                .With(new StringOneRegistry());

            var result = _construktion.Construct<string>();

            result.ShouldBe("StringTwo");
        }

        public class StringOneBlueprint : AbstractBlueprint<string>
        {
            public override string Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
            {
                return "StringOne";
            }
        }

        public class StringTwoBlueprint : AbstractBlueprint<string>
        {
            public override string Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
            {
                return "StringTwo";
            }
        }

        public class StringOneRegistry : ConstruktionRegistry
        {
            public StringOneRegistry()
            {
                AddBlueprint(new StringOneBlueprint());
            }
        }

        public class StringTwoRegistry : ConstruktionRegistry
        {
            public StringTwoRegistry()
            {
                AddBlueprint(new StringTwoBlueprint());
            }
        }
    }
}
