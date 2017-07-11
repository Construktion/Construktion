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
        public void omit_ids_should_omit_an_int_that_ends_in_Id()
        {
            _registry.OmitIds();
            var construktion = new Construktion().AddRegistry(_registry);

            var foo = construktion.Construct<Foo>();

            foo.FooId.ShouldBe(0);
        }

        [Fact]
        public void omit_ids_should_omit_a_nullable_int_that_ends_in_Id()
        {
            _registry.OmitIds();
            var construktion = new Construktion().AddRegistry(_registry);

            var foo = construktion.Construct<Foo>();

            foo.NullableFooId.ShouldBe(null);
        }

        [Fact]
        public void should_be_case_sensitive()
        {
            var registry = new BlueprintRegistry(x => x.OmitIds());
            var construktion = new Construktion().AddRegistry(registry);

            var foo = construktion.Construct<Foo>();

            foo.Fooid.ShouldNotBe(0);
        }

        [Fact]
        public void should_be_able_to_define_a_custom_convention()
        {
            var registry = new BlueprintRegistry(x => x.OmitProperties(prop => prop.EndsWith("_Id"), typeof(string)));
            var construktion = new Construktion().AddRegistry(registry);

            var foo = construktion.Construct<Foo>();

            foo.String_Id.ShouldBe(null);
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
            _registry.AddBlueprint(new StringOneBlueprint());

            var result = new Construktion().AddRegistry(_registry).Construct<string>();

            result.ShouldBe("StringOne");
        }

        [Fact]
        public void should_register_via_generic_parameter()
        {
            _registry.AddBlueprint<StringOneBlueprint>();

            var result = new Construktion().AddRegistry(_registry).Construct<string>();

            result.ShouldBe("StringOne");
        }

        [Fact]
        public void blue_prints_registered_first_are_chosen_first()
        {
            _registry.AddBlueprint(new StringTwoBlueprint());
            _registry.AddBlueprint(new StringOneBlueprint());

            var result = new Construktion().AddRegistry(_registry).Construct<string>();

            result.ShouldBe("StringTwo");
        }

        [Fact]
        public void registries_registered_first_should_have_their_blueprints_used_first()
        {
            var construktion = new Construktion();
            construktion.AddRegistry(new StringTwoRegistry());
            construktion.AddRegistry(new StringOneRegistry());

            var result = construktion.Construct<string>();

            result.ShouldBe("StringTwo");
        }

        [Fact]
        public void should_register_attribute_blueprint()
        {
            _registry.AddAttributeBlueprint<Set>(x => x.Value);

            var foo = new Construktion().AddRegistry(_registry).Construct<Foo>();

            foo.Bar.ShouldBe("SetFromAttribute");
        }

        [Fact]
        public void should_work_with_a_custom_registry()
        {
            var construktion = new Construktion().AddRegistry(new StringOneRegistry());

            var foo = construktion.Construct<Foo>();

            foo.Bar.ShouldBe("StringOne");
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
            var registryA = new StringOneRegistry();
            registryA.UseModestCtor();

            var registryB = new StringOneRegistry();

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
            var registryA = new StringOneRegistry();
            registryA.UseModestCtor();

            var registryB = new StringOneRegistry();
            registryB.UseGreedyCtor();

            var construktion = new Construktion();
            construktion.AddRegistry(registryA);
            construktion.AddRegistry(registryB);

            //act
            var result = construktion.Construct<MultiCtor>();

            result.UsedGreedyCtor.ShouldBe(true);
        }

        [Fact]
        public void should_opt_in_to_constructing_properties_with_private_setter()
        {
            var construktion = new Construktion().AddRegistry(x => x.ConstructPrivateSetters());

            var result = construktion.Construct<Foo>();

            result.StringWithPrivateSetter.ShouldNotBeNullOrWhiteSpace();
        }

        [Fact]
        public void should_use_linq_created_registry()
        {
            var construktion = new Construktion().AddRegistry(x => x.UseModestCtor());

            var result = construktion.Construct<MultiCtor>();

            result.UsedModestCtor.ShouldBe(true);
        }

        public class StringOneBlueprint : Blueprint
        {
            public bool Matches(ConstruktionContext context)
            {
                return context.RequestType == typeof(string);
            }

            public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
            {
                return "StringOne";
            }
        }

        public class StringTwoBlueprint : Blueprint
        {
            public bool Matches(ConstruktionContext context)
            {
                return context.RequestType == typeof(string);
            }

            public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
            {
                return "StringTwo";
            }
        }

        public class StringOneRegistry : BlueprintRegistry
        {
            public StringOneRegistry()
            {
                AddBlueprint(new StringOneBlueprint());
            }
        }

        public class StringTwoRegistry : BlueprintRegistry
        {
            public StringTwoRegistry()
            {
                AddBlueprint(new StringTwoBlueprint());
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
            public int? NullableFooId { get; set; }
            public int Fooid { get; set; }
            public string String_Id { get; set; }

            [Set("SetFromAttribute")]
            public string Bar { get; set; }

            public string StringWithPrivateSetter { get; private set; }
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