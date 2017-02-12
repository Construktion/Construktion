namespace Construktion
{
    using System;
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
