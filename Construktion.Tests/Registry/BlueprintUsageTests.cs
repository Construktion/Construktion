using Construktion.Blueprints;
using Shouldly;
using Xunit;

namespace Construktion.Tests.Registry
{
    public class BlueprintUsageTests
    {
        private readonly ConstruktionRegistry registry;
        private readonly Construktion construktion;

        public BlueprintUsageTests()
        {
            registry = new ConstruktionRegistry();
            construktion = new Construktion();
        }

        [Fact]
        public void should_register_a_custom_blueprint()
        {
            registry.AddBlueprint(new StringOneBlueprint());

            var result = construktion.With(registry).Construct<string>();

            result.ShouldBe("StringOne");
        }

        [Fact]
        public void should_register_via_generic_parameter()
        {
            registry.AddBlueprint<StringOneBlueprint>();

            var result = construktion.With(registry).Construct<string>();

            result.ShouldBe("StringOne");
        }

        [Fact]
        public void blue_prints_registered_first_are_chosen_first()
        {
            registry.AddBlueprint(new StringTwoBlueprint());
            registry.AddBlueprint(new StringOneBlueprint());

            var result = construktion.With(registry).Construct<string>();

            result.ShouldBe("StringTwo");
        }

        [Fact]
        public void registries_registered_first_should_have_their_blueprints_used_first()
        {
            construktion
                .With(new StringTwoRegistry())
                .With(new StringOneRegistry());

            var result = construktion.Construct<string>();

            result.ShouldBe("StringTwo");
        }

        [Fact]
        public void should_be_linq_enabled()
        {
            var reg = new ConstruktionRegistry(x => x.AddBlueprint<StringOneBlueprint>());

            var result = construktion.With(reg).Construct<string>();

            result.ShouldBe("StringOne");
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
