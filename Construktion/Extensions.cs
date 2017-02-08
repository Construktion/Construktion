namespace Construktion
{
    using System;
    using System.Linq;
    using System.Reflection;

    public static class Extensions
    {
        public static bool HasAttribute<T>(this RequestContext context) where T : Attribute
        {
            return context.PropertyInfo.HasValue() &&
                   context.PropertyInfo
                   .Single()
                   .GetCustomAttributes(typeof(T))
                   .ToList()
                   .Any();
        }

        internal static void ThrowIfNull<T>(this T item, string param) where T : class
        {
            if (item == null)
                throw new ArgumentNullException(param);
        }
    }
}
