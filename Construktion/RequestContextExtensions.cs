namespace Construktion
{
    using System;
    using System.Linq;
    using System.Reflection;

    public static class RequestContextExtensions
    {
        public static bool HasAttribute<T>(this RequestContext context) where T : Attribute
        {
            return context.PropertyInfo.HasValue() &&
                   context.PropertyInfo.Single().GetCustomAttributes(typeof(T)).ToList().Any();
        }
    }
}
