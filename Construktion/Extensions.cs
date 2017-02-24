namespace Construktion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class Extensions
    {
        public static ConstructorInfo Greediest(this List<ConstructorInfo> ctors)
        {
            var max = ctors.Max(x => x.GetParameters().Length);
            var greedyCtor = ctors.First(x => x.GetParameters().Length == max);

            return greedyCtor;
        }

        public static bool HasDefaultCtor(this Type type)
        {
            var ctors = type.GetTypeInfo()
             .DeclaredConstructors
             .ToList();

            return ctors.Any(x => x.GetParameters().Length == 0);
        }

        public static bool HasNonDefaultCtor(this Type type)
        {
            var ctors = type.GetTypeInfo()
             .DeclaredConstructors
             .ToList();

            return ctors.Any(x => x.GetParameters().Length != 0);
        }
    }
}
