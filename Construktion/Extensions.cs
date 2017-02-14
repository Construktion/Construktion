namespace Construktion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class Extensions
    {
        public static bool HasAttribute<T>(this ConstruktionContext context) where T : Attribute
        {
            return context.PropertyInfo.HasValue() &&
                   context.PropertyInfo
                   .Single()
                   .GetCustomAttributes(typeof(T))
                   .ToList()
                   .Any();
        }

        public static ConstructorInfo GetGreedyCtor(this List<ConstructorInfo> ctors)
        {
            var max = ctors.Max(x => x.GetParameters().Length);
            var greedyCtor = ctors.First(x => x.GetParameters().Length == max);

            return greedyCtor;
        }

        internal static bool HasDefaultCtor(this Type type)
        {
            var ctors = type.GetTypeInfo()
             .DeclaredConstructors
             .ToList();

            return ctors.Any(x => x.GetParameters().Length == 0);
        }

        internal static void ThrowIfNull<T>(this T item, string param) where T : class
        {
            if (item == null)
                throw new ArgumentNullException(param);
        }
    }
}
