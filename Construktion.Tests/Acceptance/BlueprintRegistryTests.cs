namespace Construktion.Tests.Acceptance
{
    using System;
    using global::Construktion.Blueprints;
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
        public void should_register_a_custom_blueprint()
        {
            _registry.AddBlueprint(new HardCodeStringBlueprint());

            var result = new Construktion(_registry).Construct<Foo>();

            result.Bar.ShouldBe("HardCode");
        }

        [Fact]
        public void lambda_expression_should_register_container()
        {
            _registry.AddContainerBlueprint(x => x.Register<IFoo, Foo>());

            var result = new Construktion(_registry).Construct<IFoo>();

            result.ShouldBeOfType<Foo>();
        }

        [Fact]
        public void supplied_instance_should_register_container()
        {
            var container = new SimpleContainer();
            container.Register<IFoo, Foo>();
            _registry.AddContainerBlueprint(container);

            var result = new Construktion(_registry).Construct<IFoo>();

            result.ShouldBeOfType<Foo>();
        }

        [Fact]
        public void should_register_attribute_blueprint()
        {
            _registry.AddAttributeBlueprint<Set>(x => x.Value);

            var result = new Construktion(_registry).Construct<Foo>();

            result.Bar.ShouldBe("Set");
        }

        [Fact]
        public void should_work_with_a_custom_registry()
        {
            var construktion = new Construktion(new HardCodeRegistry());

            var result = construktion.Construct<Foo>();

            result.Bar.ShouldBe("HardCode");
        }

        public class HardCodeStringBlueprint : Blueprint
        {
            public bool Matches(ConstruktionContext context)
            {
                return context.PropertyInfo != null && context.PropertyInfo.PropertyType == typeof(string);
            }

            public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
            {
                return "HardCode";
            }
        }

        public class HardCodeRegistry : BlueprintRegistry
        {
            public HardCodeRegistry()
            {
                AddBlueprint(new HardCodeStringBlueprint());
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
    }
}