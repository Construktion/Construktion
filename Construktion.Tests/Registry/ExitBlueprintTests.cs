using System;
using Construktion.Blueprints;
using Shouldly;
using Xunit;

namespace Construktion.Tests.Registry
{
    public class ExitBlueprintTests
    {
        private readonly ConstruktionRegistry registry;
        private readonly Construktion construktion;

        public ExitBlueprintTests()
        {
            registry = new ConstruktionRegistry();
            construktion = new Construktion();
        }

        [Fact]
        public void should_register_a_custom_exit_blueprint()
        {
            registry.AddExitBlueprint(new StringExitOneBlueprint());

            var result = construktion.With(registry).Construct<string>();

            result.ShouldEndWith("One");
        }

        [Fact]
        public void should_register_via_generic_parameter()
        {
            registry.AddExitBlueprint<StringExitOneBlueprint>();

            var result = construktion.With(registry).Construct<string>();

            result.ShouldEndWith("One");
        }

        [Fact]
        public void exit_blueprints_registered_first_are_chosen_first()
        {
            registry.AddExitBlueprint<StringExitTwoBlueprint>();
            registry.AddExitBlueprint<StringExitOneBlueprint>();

            var result = construktion.With(registry).Construct<string>();

            result.ShouldEndWith("Two");
        }

        [Fact]
        public void registries_registered_first_should_have_their_exit_blueprints_used_first()
        {
            construktion
                .With(new StringTwoRegistry())
                .With(new StringOneRegistry());

            var result = construktion.Construct<string>();

            result.ShouldEndWith("Two");
        }

        [Fact]
        public void should_be_linq_enabled()
        {
            var _registry = new ConstruktionRegistry(x => x.AddExitBlueprint<StringExitOneBlueprint>());

            var result = construktion.With(_registry).Construct<string>();

            result.ShouldEndWith("One");
        }

        public class StringExitOneBlueprint : AbstractExitBlueprint<string>
        {
            public override string Construct(string item, ConstruktionPipeline pipeline)
            {
                return item + "One";
            }
        }

        public class StringExitTwoBlueprint : AbstractExitBlueprint<string>
        {
            public override string Construct(string item, ConstruktionPipeline pipeline)
            {
                return item + "Two";
            }
        }

        public class StringOneRegistry : ConstruktionRegistry
        {
            public StringOneRegistry()
            {
                AddExitBlueprint<StringExitOneBlueprint>();
            }
        }

        public class StringTwoRegistry : ConstruktionRegistry
        {
            public StringTwoRegistry()
            {
                AddExitBlueprint<StringExitTwoBlueprint>();
            }
        }
    }
}