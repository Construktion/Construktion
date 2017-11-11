namespace Construktion.Internal
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    //https://stackoverflow.com/questions/12307519/activator-createinstancet-vs-compiled-expression-inverse-performance-on-two-da
    internal static class ReflectionCache
    {
        private delegate object Ctor(params object[] args);

        private delegate void Setter(object instance, object value);

        private delegate object NewGenericType();

        private static readonly IDictionary<string, Ctor> _ctorCache = new ConcurrentDictionary<string, Ctor>();

        private static readonly IDictionary<PropertyInfo, Setter> _setterCache =
            new ConcurrentDictionary<PropertyInfo, Setter>();

        private static readonly IDictionary<string, NewGenericType> _genericTypeCache =
            new ConcurrentDictionary<string, NewGenericType>();

        public static object NewUp(this Type input, params object[] args)
        {
            var cacheKey = input.AssemblyQualifiedName + string.Join("", args.Select(x => x.ToString()));

            if (_ctorCache.TryGetValue(cacheKey, out Ctor ctor))
                return ctor(args);

            var types = args.Select(p => p.GetType());
            var constructor = input.GetConstructor(types.ToArray());

            var paramInfo = constructor.GetParameters();

            var paramEx = Expression.Parameter(typeof(object[]), "args");

            var argEx = new Expression[paramInfo.Length];
            for (var i = 0; i < paramInfo.Length; i++)
            {
                var index = Expression.Constant(i);
                var paramType = paramInfo[i].ParameterType;
                var accessor = Expression.ArrayIndex(paramEx, index);
                var cast = Expression.Convert(accessor, paramType);
                argEx[i] = cast;
            }

            var lambda = Expression.Lambda(typeof(Ctor), Expression.New(constructor, argEx), paramEx);

            ctor = (Ctor)lambda.Compile();

            _ctorCache[cacheKey] = ctor;

            return ctor(args);
        }

        public static void SetPropertyValue(this PropertyInfo propertyInfo, object instance, object value)
        {
            if (value == null)
                return;

            if (_setterCache.TryGetValue(propertyInfo, out Setter setter))
            {
                setter(instance, value);
                return;
            }

            var instanceParam = Expression.Parameter(typeof(object));
            var valueParam = Expression.Parameter(typeof(object));

            var instanceCast = Expression.Convert(instanceParam, propertyInfo.DeclaringType);
            var valueCast = Expression.Convert(valueParam, propertyInfo.PropertyType);
            var lambda = Expression.Lambda(typeof(Setter),
                Expression.Call(instanceCast, propertyInfo.GetSetMethod(true), valueCast),
                new ParameterExpression[] { instanceParam, valueParam });

            setter = (Setter)lambda.Compile();

            _setterCache[propertyInfo] = setter;

            setter(instance, value);
        }

        //https://stackoverflow.com/a/20734641/2612547
        public static object NewGeneric(this Type genericTypeDefinition, params Type[] genericParameters)
        {
            var cacheKey = genericTypeDefinition.AssemblyQualifiedName +
                           string.Join("", genericParameters.Select(x => x.ToString()));

            if (_genericTypeCache.TryGetValue(cacheKey, out NewGenericType makeGenericType))
                return makeGenericType();

            var genericType = genericTypeDefinition.MakeGenericType(genericParameters);

            makeGenericType = Expression.Lambda<NewGenericType>(Expression.New(genericType)).Compile();

            _genericTypeCache[cacheKey] = makeGenericType;

            return makeGenericType();
        }
    }
}