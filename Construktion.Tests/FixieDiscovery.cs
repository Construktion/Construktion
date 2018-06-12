namespace Construktion.Tests
{
    using System;
    using Fixie;

    public class FixieDiscovery : Discovery
    {
        public FixieDiscovery()
        {
            Classes
                .Where(x => x.Name.EndsWith("Tests") || isBugTest(x));

            Methods
                .Where(method => method.IsPublic && method.IsVoid());

            bool isBugTest(Type @class) => @class.Namespace.StartsWith("Construktion.Tests.Bugs") &&
                                           @class.IsNested == false;
        }
    }
}