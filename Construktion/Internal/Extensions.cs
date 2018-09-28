using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Construktion.Tests")]
[assembly: InternalsVisibleTo("Construktion.Benchmarks")]

namespace Construktion.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    internal static class Extensions
    {
        public static ConstructorInfo GreedyCtor(this IEnumerable<ConstructorInfo> ctors)
        {
            var max = ctors.Max(x => x.GetParameters().Length);
            var greedyCtor = ctors.First(x => x.GetParameters().Length == max);

            return greedyCtor;
        }

        public static ConstructorInfo ModestCtor(this IEnumerable<ConstructorInfo> ctors)
        {
            var min = ctors.Min(x => x.GetParameters().Length);
            var modestCtor = ctors.First(x => x.GetParameters().Length == min);

            return modestCtor;
        }

        public static IEnumerable<PropertyInfo> PropertiesWithPublicSetter(Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                       .Where(x => x.CanWrite && x.GetSetMethod( /*nonPublic*/ true).IsPublic);
        }

        public static IEnumerable<PropertyInfo> PropertiesWithAccessibleSetter(Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                       .Where(x => x.CanWrite);
        }

        public static PropertyInfo NulloPropertyInfo = typeof(Unit).GetProperty(nameof(Unit.NulloProperty));

        public static ParameterInfo NulloParameterInfo = typeof(Unit)
                                                    .GetMethod(nameof(Unit.NulloMethod))
                                                    .GetParameters()
                                                    .Single();

        public static bool IsNulloPropertyInfo(this PropertyInfo propertyInfo)
        {
            return propertyInfo.DeclaringType == typeof(Unit) &&
                   propertyInfo.Name == nameof(Unit.NulloProperty) &&
                   propertyInfo.PropertyType == typeof(Nullo);
        }

        public static bool IsNulloParameterInfo(this ParameterInfo parameterInfo)
        {
            var nulloParameterName = typeof(Unit)
                                     .GetMethod(nameof(Unit.NulloMethod))
                                     .GetParameters()
                                     .Single()
                                     .Name;

            return parameterInfo.ParameterType == typeof(Nullo) &&
                   parameterInfo.Name == nulloParameterName;
        }
    }
}