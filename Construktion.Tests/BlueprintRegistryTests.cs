namespace Construktion.Tests
{
    using System;
    using Blueprints;
    using Recursive;
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

            result.Foo.ShouldNotBe(0);
            result.Bar.ShouldNotBe(0);
        }

        [Fact]
        public void should_use_modest_ctor_when_opted_in()
        {
            _blueprintRegistry.UseModestCtor();

            var result = new Construktion().AddRegistry(_blueprintRegistry).Construct<MultiCtor>();

            result.Foo.ShouldNotBe(0);
            result.Bar.ShouldBe(0);
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
            public int Foo { get; }
            public int Bar { get; }

            public MultiCtor(int foo)
            {
                Foo = foo;
            }

            public MultiCtor(int foo, int bar)
            {
                Foo = foo;
                Bar = bar;
            }
        }
    }
}