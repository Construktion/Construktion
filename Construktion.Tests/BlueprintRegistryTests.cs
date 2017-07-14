namespace Construktion.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Blueprints;
    using Shouldly;
    using Xunit;

    public class BlueprintRegistryTests
    {
        private readonly BlueprintRegistry _registry;
        private readonly Construktion _construktion;

        public BlueprintRegistryTests()
        {
            _registry = new BlueprintRegistry();
            _construktion = new Construktion();
        }

        [Fact]
        public void should_register_a_custom_blueprint()
        {
            _registry.AddBlueprint(new StringOneBlueprint());

            var result = _construktion.AddRegistry(_registry).Construct<string>();

            result.ShouldBe("StringOne");
        }

        [Fact]
        public void omit_ids_should_omit_an_int_that_ends_in_Id()
        {
            _registry.OmitIds();
            var construktion = _construktion.AddRegistry(_registry);

            var foo = construktion.Construct<Foo>();

            foo.FooId.ShouldBe(0);
        }

        [Fact]
        public void omit_ids_should_omit_a_nullable_int_that_ends_in_Id()
        {
            _registry.OmitIds();
            var construktion = _construktion.AddRegistry(_registry);

            var foo = construktion.Construct<Foo>();

            foo.NullableFooId.ShouldBe(null);
        }

        [Fact]
        public void should_be_case_sensitive()
        {
            var registry = new BlueprintRegistry(x => x.OmitIds());
            var construktion = _construktion.AddRegistry(registry);

            var foo = construktion.Construct<Foo>();

            foo.Fooid.ShouldNotBe(0);
        }

        [Fact]
        public void should_be_able_to_define_a_custom_convention()
        {
            var registry = new BlueprintRegistry(x => x.OmitProperties(prop => prop.EndsWith("_Id"), typeof(string)));
            var construktion = _construktion.AddRegistry(registry);

            var foo = construktion.Construct<Foo>();

            foo.String_Id.ShouldBe(null);
        }

        [Fact]
        public void should_register_via_generic_parameter()
        {
            _registry.AddBlueprint<StringOneBlueprint>();

            var result = _construktion.AddRegistry(_registry).Construct<string>();

            result.ShouldBe("StringOne");
        }

        [Fact]
        public void blue_prints_registered_first_are_chosen_first()
        {
            _registry.AddBlueprint(new StringTwoBlueprint());
            _registry.AddBlueprint(new StringOneBlueprint());

            var result = _construktion.AddRegistry(_registry).Construct<string>();

            result.ShouldBe("StringTwo");
        }

        [Fact]
        public void registries_registered_first_should_have_their_blueprints_used_first()
        {
            var construktion = _construktion;
            construktion.AddRegistry(new StringTwoRegistry());
            construktion.AddRegistry(new StringOneRegistry());

            var result = construktion.Construct<string>();

            result.ShouldBe("StringTwo");
        }

        [Fact]
        public void should_work_with_a_custom_registry()
        {
            var construktion = _construktion.AddRegistry(new StringOneRegistry());

            var foo = construktion.Construct<Foo>();

            foo.Bar.ShouldBe("StringOne");
        }

        [Fact]
        public void should_register_instance_with_contract()
        {
            _registry.Register<IFoo, Foo>();

            var result = _construktion.AddRegistry(_registry).Construct<IFoo>();

            result.ShouldBeOfType<Foo>();
        }

        [Fact]
        public void last_registered_instance_should_be_chosen()
        {
            _registry.Register<IFoo, Foo>();
            _registry.Register<IFoo, Foo2>();


            var result = _construktion.AddRegistry(_registry).Construct<IFoo>();

            result.ShouldBeOfType<Foo2>();
        }

        [Fact]
        public void should_register_scoped_instance()
        {
            var foo = new Foo { FooId = -1, String_Id = "-1" };
            _registry.RegisterScoped<IFoo>(foo);

            var result = _construktion.AddRegistry(_registry).Construct<IFoo>();

            var fooResult = result.ShouldBeOfType<Foo>();
            fooResult.FooId.ShouldBe(-1);
            fooResult.String_Id.ShouldBe("-1");
            fooResult.GetHashCode().ShouldBe(foo.GetHashCode());
        }

        [Fact]
        public void should_not_matter_where_scoped_instance_is_in_the_graph()
        {
            var foo = new Foo();
            var registry = new BlueprintRegistry();
            registry.RegisterScoped<IFoo>(foo);

            var construktion = _construktion.AddRegistry(registry);

            var result = construktion.Construct<LovesFoo>();

            result.CtorFoo.GetHashCode().ShouldBe(foo.GetHashCode());
            result.PropertyFoo.GetHashCode().ShouldBe(foo.GetHashCode());
        }

        [Fact]
        public void should_throw_when_interface_isnt_registered()
        {
            //_blueprintRegistry.Register<IFoo, Foo>();

            Should.Throw<Exception>
                (() => _construktion.AddRegistry(_registry).Construct<IFoo>())
                .Message
                .ShouldContain("Cannot construct the interface IFoo.");
        }

        [Fact]
        public void should_resolve_greediest_ctor_by_default()
        {
            var result = _construktion.Construct<MultiCtor>();

            result.UsedGreedyCtor.ShouldBe(true);
        }

        [Fact]
        public void should_use_modest_ctor_when_opted_in()
        {
            _registry.UseModestCtor();

            var result = _construktion.AddRegistry(_registry).Construct<MultiCtor>();

            result.UsedModestCtor.ShouldBe(true);
        }

        [Fact]
        public void a_new_registry_without_a_ctor_strategy_should_not_overwrite_previous()
        {
            var registryA = new StringOneRegistry();
            registryA.UseModestCtor();

            var registryB = new StringOneRegistry();

            var construktion = _construktion;
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

            var construktion = _construktion;
            construktion.AddRegistry(registryA);
            construktion.AddRegistry(registryB);

            //act
            var result = construktion.Construct<MultiCtor>();

            result.UsedGreedyCtor.ShouldBe(true);
        }

        [Fact]
        public void should_opt_in_to_constructing_properties_with_private_setter()
        {
            var construktion = _construktion.AddRegistry(x => x.ConstructPrivateSetters());

            var result = construktion.Construct<Foo>();

            result.StringWithPrivateSetter.ShouldNotBeNullOrWhiteSpace();
        }

        [Fact]
        public void should_overwrite_previous_property_selector()
        {
            _registry
                .ConstructPrivateSetters()
                .OmitPrivateSetters();

            var construktion = _construktion.AddRegistry(_registry);

            var result = construktion.Construct<Foo>();

            result.StringWithPrivateSetter.ShouldBeNullOrWhiteSpace();
        }

        [Fact]
        public void should_use_linq_created_registry()
        {
            var construktion = _construktion.AddRegistry(x => x.UseModestCtor());

            var result = construktion.Construct<MultiCtor>();

            result.UsedModestCtor.ShouldBe(true);
        }

        [Fact]
        public void should_register_property_attribute_blueprint()
        {
            _registry.AddPropertyAttributeBlueprint<Set>(x => x.Value);

            var foo = _construktion.AddRegistry(_registry).Construct<Foo>();

            foo.Bar.ShouldBe("Set");
        }

        //todo need to refactor this and tests simliar to this
        [Fact]
        public void new_registry_should_not_overwrite_previous_enumerable_count()
        {
            _registry.EnumerableCount(1);
            _registry.AddRegistry(new BlueprintRegistry());

            var ints = _construktion.AddRegistry(_registry).ConstructMany<int>();

            ints.Count().ShouldBe(1);
        }

        //config
        [Fact]
        public void new_registry_should_not_overwrite_previous_enumerable_count_but_it_should_if_its_Set()
        {
            _registry.EnumerableCount(1);
            _registry.AddRegistry(new BlueprintRegistry(x => x.EnumerableCount(2)));

            var ints = _construktion.AddRegistry(_registry).ConstructMany<int>();

            ints.Count().ShouldBe(2);
        }

        [Fact]
        public void should_register_parameter_attribute_blueprint()
        {
            _registry.AddParameterAttributeBlueprint<Set>(x => x.Value);

            var parameterInfo =
                typeof(BlueprintRegistryTests).GetMethod(nameof(TestMethod))
                    .GetParameters()
                    .Single();

            var parameter = (string)_construktion.AddRegistry(_registry).Construct(parameterInfo);

            parameter.ShouldBe("Set");
        }

        public void TestMethod([Set("Set")] string parameter) { }

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

            [Set("Set")]
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

        public class LovesFoo
        {
            public IFoo CtorFoo { get; }
            public IFoo PropertyFoo { get; set; }

            public LovesFoo(IFoo foo)
            {
                CtorFoo = foo;
            }
        }
    }
}