namespace Construktion.Samples
{
    using System.Collections.Generic;
    using Entities;

    public class SamplesRegistry : ConstruktionRegistry
    {
        public SamplesRegistry()
        {
            OmitIds();
            OmitProperties(typeof(IEnumerable<>));
            ConstructPropertyUsing(pi => pi.Name.Equals("IsActive") &&
                                         pi.PropertyType == typeof(bool), () => true);

            ConstructPropertyUsing(pi => pi.PropertyType == typeof(bool), () => false);
            AddBlueprint<FakeBuilderBlueprint>();
            Register<Service, TestService>();
        }
    }
}