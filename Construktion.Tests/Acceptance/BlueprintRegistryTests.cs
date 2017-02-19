namespace Construktion.Tests.Acceptance
{
    using System;
    using global::Construktion.Blueprints;
    using Shouldly;
    using Xunit;

    public class BlueprintRegistryTests
    {
     
        [Fact]
        public void should_register_a_custom_blueprint()
        {
            var registry = new BlueprintRegistry();
            registry.AddBlueprint(new HardCodeStringBlueprint());
            
            var foo = new Construktion(registry).Construct<Foo>();

            foo.Bar.ShouldBe("HardCode");
        }

        [Fact]
        public void should_register_container()
        {
            var container = new SimpleContainer();
            container.Register<IFoo, Foo>();
            var registry = new BlueprintRegistry();
            registry.AddContainerBlueprint(container);

            var result = new Construktion(registry).Construct<IFoo>();

            result.ShouldBeOfType<Foo>();
        }

        [Fact]
        public void should_register_container_with_lambda()
        {
            var registry = new BlueprintRegistry();
            registry.AddContainerBlueprint(x => x.Register<IFoo, Foo>());

            var result = new Construktion(registry).Construct<IFoo>();

            result.ShouldBeOfType<Foo>();
        }

        [Fact]
        public void should_register_attribute_blueprint()
        {
            var registry = new BlueprintRegistry();
            registry.AddAttributeBlueprint<Set>(x => x.Value);

            var foo = new Construktion(registry).Construct<Foo>();

            foo.Bar.ShouldBe("Set");
        }

        [Fact]
        public void should_work_with_a_custom_registry()
        {
            var construktion = new Construktion(new HardCodeRegistry());

            var foo = construktion.Construct<Foo>();

            foo.Bar.ShouldBe("HardCode");
        }

        public class HardCodeRegistry : BlueprintRegistry
        {
            public HardCodeRegistry()
            {
                AddBlueprint(new HardCodeStringBlueprint());
            }
        }

        public class HardCodeStringBlueprint : Blueprint
        {
            public bool Matches(ConstruktionContext context)
            {
                return context.PropertyContext.IsType(typeof(string));
            }

            public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
            {
                return "HardCode";
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