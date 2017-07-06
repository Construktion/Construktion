﻿namespace Construktion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class Extensions
    {
        public static ConstructorInfo GreedyCtor(this List<ConstructorInfo> ctors)
        {
            var max = ctors.Max(x => x.GetParameters().Length);
            var greedyCtor = ctors.First(x => x.GetParameters().Length == max);

            return greedyCtor;
        }

        public static ConstructorInfo ModestCtor(this List<ConstructorInfo> ctors)
        {
            var min = ctors.Min(x => x.GetParameters().Length);
            var modestCtor = ctors.First(x => x.GetParameters().Length == min);

            return modestCtor;
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

        public static List<T> Replace<T>(this List<T> items, Type item, T newItem)
        {
            var idx = items.FindIndex(x => x.GetType() == item);

            items[idx] = newItem;

            return items;
        }

        public static IEnumerable<PropertyInfo> PropertiesWithPublicSetter(Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.CanWrite && x.GetSetMethod(/*nonPublic*/ true).IsPublic);
        }

        public static IEnumerable<PropertyInfo> PropertiesWithAccessibleSetter(Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.CanWrite);
        }
    }
}
