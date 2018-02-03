namespace Construktion.Tests.Registry
{
    using System;
    using System.Linq;
    using Shouldly;

    public class ParameterAttributeTests
    {
        [Fact]
        public void should_register_parameter_attribute_blueprint()
        {
            var registry = new ConstruktionRegistry().AddParameterAttribute<Set>(x => x.Value);

            var parameterInfo =
                typeof(ParameterAttributeTests).GetMethod(nameof(ParameterTest))
                                               .GetParameters()
                                               .Single();

            var parameter = (string)new Construktion().With(registry).Construct(parameterInfo);

            parameter.ShouldBe("Set");
        }

        public void ParameterTest([Set("Set")] string parameter) { }

        public class Set : Attribute
        {
            public string Value { get; }

            public Set(string value)
            {
                Value = value;
            }
        }
    }
}