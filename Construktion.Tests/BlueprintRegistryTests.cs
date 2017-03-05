namespace Construktion.Tests
{
    using System;
    using Blueprints;
    using Shouldly;
    using Xunit;

    public class BlueprintRegistryTests
    {
        private readonly BlueprintRegistry _blueprintRegistry;

        public BlueprintRegistryTests()
        {
            _blueprintRegistry = new BlueprintRegistry();
        }

        [Fact]
        public void should_register_instance_with_contract()
        {
            _blueprintRegistry.Register<IFoo, Foo>();

            var result = new Construktion().AddRegistry(_blueprintRegistry).Construct<IFoo>();

            result.ShouldBeOfType<Foo>();
        }

        [Fact]
        public void should_register_a_custom_blueprint()
        {
            _blueprintRegistry.AddBlueprint(new StringA());

            var result = new Construktion().AddRegistry(_blueprintRegistry).Construct<string>();

            result.ShouldBe("StringA");
        }

        [Fact]
        public void should_register_via_generic_parameter()
        {
            _blueprintRegistry.AddBlueprint<StringA>();

            var result = new Construktion().AddRegistry(_blueprintRegistry).Construct<string>();

            result.ShouldBe("StringA");
        }

        [Fact]
        public void blue_prints_registered_first_are_chosen_first()
        {
            _blueprintRegistry.AddBlueprint(new StringB());
            _blueprintRegistry.AddBlueprint(new StringA());

            var result = new Construktion().AddRegistry(_blueprintRegistry).Construct<string>();

            result.ShouldBe("StringB");
        }

        [Fact]
        public void registries_registered_first_have_their_blueprints_used_first()
        {
            var construktion = new Construktion();
            construktion.AddRegistry(new StringBRegistry());
            construktion.AddRegistry(new StringARegistry());

            var result = construktion.Construct<string>();

            result.ShouldBe("StringB");
        }

        [Fact]
        public void should_register_attribute_blueprint()
        {
            _blueprintRegistry.AddAttributeBlueprint<Set>(x => x.Value);

            var foo = new Construktion().AddRegistry(_blueprintRegistry).Construct<Foo>();

            foo.Bar.ShouldBe("Set");
        }

        [Fact]
        public void should_work_with_a_custom_registry()
        {
            var construktion = new Construktion().AddRegistry(new StringARegistry());

            var foo = construktion.Construct<Foo>();

            foo.Bar.ShouldBe("StringA");
        }

        [Fact]
        public void should_throw_when_interface_isnt_registered()
        {
            //_blueprintRegistry.Register<IFoo, Foo>();

            Should.Throw<Exception>
                (() => new Construktion().AddRegistry(_blueprintRegistry).Construct<IFoo>())
                .Message
                .ShouldContain("Cannot construct the interface IFoo.");
        }

        [Fact]
        public void should_resolve_greediest_ctor_by_default()
        {
            var result = new Construktion().Construct<MultiCtor>();

            result.UsedGreedyCtor.ShouldBe(true);
        }

        [Fact]
        public void should_use_modest_ctor_when_opted_in()
        {
            _blueprintRegistry.UseModestCtor();

            var result = new Construktion().AddRegistry(_blueprintRegistry).Construct<MultiCtor>();

            result.UsedModestCtor.ShouldBe(true);
        }


        [Fact]
        public void should_respect_registries_ctor_strategy()
        {
            var registryA = new StringARegistry();
            registryA.UseModestCtor();
            var registryB = new StringARegistry();
            var construktion = new Construktion();
            construktion.AddRegistry(registryA);
            construktion.AddRegistry(registryB);

            var result = construktion.Construct<MultiCtor>();

            result.UsedModestCtor.ShouldBe(true);
        }

        [Fact]
        public void registries_ctor_strategy_should_overwrite_previous()
        {
            var registryA = new StringARegistry();
            registryA.UseModestCtor();
            var registryB = new StringARegistry();
            registryB.UseGreedyCtor();
            var construktion = new Construktion();
            construktion.AddRegistry(registryA);
            construktion.AddRegistry(registryB);

            var result = construktion.Construct<MultiCtor>();

            result.UsedGreedyCtor.ShouldBe(true);
        }

        [Fact]
        public void should_use_linq_created_registry()
        {
            var construktion = new Construktion().AddRegistry(x => x.UseModestCtor());

            var result = construktion.Construct<MultiCtor>();

            result.UsedModestCtor.ShouldBe(true);
        }

        public class StringA : Blueprint
        {
            public bool Matches(ConstruktionContext context)
            {
                return context.RequestType == typeof(string);
            }

            public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
            {
                return "StringA";
            }
        }

        public class StringB : Blueprint
        {
            public bool Matches(ConstruktionContext context)
            {
                return context.RequestType == typeof(string);
            }

            public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
            {
                return "StringB";
            }
        }

        public class StringARegistry : BlueprintRegistry
        {
            public StringARegistry()
            {
                AddBlueprint(new StringA());
            }
        }

        public class StringBRegistry : BlueprintRegistry
        {
            public StringBRegistry()
            {
                AddBlueprint(new StringB());
            }
        }

        public class Set : Attribute
        {
            public string Value { get; }

            public Set(string value)
            {
                Value = value;
            }
        }

        public interface IFoo { }

        public class Foo : IFoo
        {
            [Set("Set")]
            public string Bar { get; set; }
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