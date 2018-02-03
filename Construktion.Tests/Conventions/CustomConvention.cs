namespace Construktion.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Fixie;

    public class CustomConvention : Convention
    {
        public CustomConvention()
        {
            Classes
                .Where(HasAnyFactMethods);

            Methods
                .HasOrInherits<FactAttribute>();

            ClassExecution
                .CreateInstancePerCase()
                .ShuffleCases();
        }

        bool HasAnyFactMethods(Type type)
        {
            return type.GetMethods(BindingFlags.Public | BindingFlags.Instance).Any(x => x.HasOrInherits<FactAttribute>());
        }
    }
}