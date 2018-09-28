namespace Construktion.Tests.Registry
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Shouldly;

    public class ParameterAttributeTests
    {
        public void should_register_parameter_attribute_blueprint()
        {
            var registry = new ConstruktionRegistry().AddParameterAttribute<Set>(x => x.Value);

            var parameterInfo = typeof(ParameterAttributeTests)
                .GetMethod(nameof(ParameterTest), BindingFlags.NonPublic | BindingFlags.Instance)
                .GetParameters()
                .Single();

            var parameter = (string)new Construktion().Apply(registry).Construct(parameterInfo);

            parameter.ShouldBe("Set");
        }

        private void ParameterTest([Set("Set")] string parameter) { }

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