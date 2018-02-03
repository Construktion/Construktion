namespace Construktion.Tests
{
    using System;
    using Fixie;

    public class FixieConvention : Convention
    {
        public FixieConvention()
        {
            Classes
                .Where(x => x.Name.EndsWith("Tests") || isBugTest(x));

            Methods
                .Where(method => method.IsPublic && method.IsVoid());

            ClassExecution
                .CreateInstancePerCase()
                .ShuffleCases();

            bool isBugTest(Type @class) => @class.Namespace.StartsWith("Construktion.Tests.Bug") &&
                                           @class.IsNested == false;
        }
    }
}