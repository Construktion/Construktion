namespace Construktion.Tests
{
    using System;
    using Blueprints;
    using Shouldly;
    using Xunit;

    public class BlueprintRegistryTests
    {
        private readonly BlueprintRegistry _registry;

        public BlueprintRegistryTests()
        {
            _registry = new BlueprintRegistry();
        }

        [Fact]
        public void should_omit_ids()
        {
            _registry.OmitIds();
            var construktion = new Construktion().AddRegistry(_registry);

            var foo = construktion.Construct<Foo>();

            foo.FooId.ShouldBe(0);
        }

        [Fact]
        public void should_register_instance_with_contract()
        {
            _registry.Register<IFoo, Foo>();

            var result = new Construktion().AddRegistry(_registry).Construct<IFoo>();

            result.ShouldBeOfType<Foo>();
        }

        [Fact]
        public void last_registered_instance_should_be_chosen()
        {
            _registry.Register<IFoo, Foo>();
            _registry.Register<IFoo, Foo2>();

            var result = new Construktion().AddRegistry(_registry).Construct<IFoo>();

            result.ShouldBeOfType<Foo2>();
        }

        [Fact]
        public void should_register_a_custom_blueprint()
        {
            _registry.AddBlueprint(new StringA());

            var result = new Construktion().AddRegistry(_registry).Construct<string>();

            result.ShouldBe("StringA");
        }

        [Fact]
        public void should_register_via_generic_parameter()
        {
            _registry.AddBlueprint<StringA>();

            var result = new Construktion().AddRegistry(_registry).Construct<string>();

            result.ShouldBe("StringA");
        }

        [Fact]
        public void blue_prints_registered_first_are_chosen_first()
        {
            _registry.AddBlueprint(new StringB());
            _registry.AddBlueprint(new StringA());

            var result = new Construktion().AddRegistry(_registry).Construct<string>();

            result.ShouldBe("StringB");
        }

        [Fact]
        public void registries_registered_first_should_have_their_blueprints_used_first()
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
            _registry.AddAttributeBlueprint<Set>(x => x.Value);

            var foo = new Construktion().AddRegistry(_registry).Construct<Foo>();

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
                (() => new Construktion().AddRegistry(_registry).Construct<IFoo>())
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
            _registry.UseModestCtor();

            var result = new Construktion().AddRegistry(_registry).Construct<MultiCtor>();

            result.UsedModestCtor.ShouldBe(true);
        }

        [Fact]
        public void a_new_registry_without_a_ctor_strategy_should_not_overwrite_previous()
        {
            var registryA = new StringARegistry();
            registryA.UseModestCtor();

            var registryB = new StringARegistry();

            var construktion = new Construktion();
            construktion.AddRegistry(registryA);
            construktion.AddRegistry(registryB);

            //act
            var result = construktion.Construct<MultiCtor>();

            result.UsedModestCtor.ShouldBe(true);
        }

        [Fact]
        public void should_use_the_last_registered_ctor_strategy()
        {
            var registryA = new StringARegistry();
            registryA.UseModestCtor();

            var registryB = new StringARegistry();
            registryB.UseGreedyCtor();

            var construktion = new Construktion();
            construktion.AddRegistry(registryA);
            construktion.AddRegistry(registryB);

            //act
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
            public int FooId { get; set; }

            [Set("Set")]
            public string Bar { get; set; }
        }

        public class Foo2 : IFoo { }

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