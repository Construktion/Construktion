using Construktion.Samples.Entities;

namespace Construktion.Samples
{
    using System.Collections.Generic;
    using System.Reflection;
    using Xunit.Abstractions;
    using Xunit.Sdk;

    [DataDiscoverer("Construktion.Samples.NoDataDiscovery", "Construktion.Samples")]
    public class ConstruktionData : DataAttribute
    {
        public static readonly Construktion Construktion = new Construktion()
            .With(x =>
            {
                x.OmitIds();
                x.OmitProperties(typeof(IEnumerable<>));
                x.ConstructPropertyUsing(pi =>  pi.Name.Equals("IsActive") && 
                                                pi.PropertyType == typeof(bool),
                                                () => true);

                x.ConstructPropertyUsing(pi => pi.PropertyType == typeof(bool), () => false);
                x.AddBlueprint<FakeBuilderBlueprint>();
                x.Register<Service, TestService>();
            });

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            var parameters = new List<object>();

            foreach (var paramInfo in testMethod.GetParameters())
            {
                var result = Construktion.Construct(paramInfo);

                parameters.Add(result);
            }

            return new[] { parameters.ToArray() };
        }
    }

    public class NoDataDiscovery : DataDiscoverer
    {
        public override bool SupportsDiscoveryEnumeration(IAttributeInfo dataAttribute, IMethodInfo testMethod)
        {
            //If this was true Xunit will build out the parameters during test discovery and
            //display the constructed values in the test runner as: "TestClass.Case(param1: value, param2: value)".
            //We want to suppress that behavior since the parameters are generated dynamically, and the actual values
            //used in the test case run would be different than what's displayed in the test runner.
            return false;
        }
    }
}