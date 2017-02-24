namespace Construktion.Blueprints.Recursive
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public class NonEmptyCtorBlueprint : Blueprint
    {
        private readonly Dictionary<Type, Type> _typeMap;

        public NonEmptyCtorBlueprint(Dictionary<Type, Type> typeMap)
        {
            if (typeMap == null)
                throw new ArgumentNullException(nameof(typeMap));

            _typeMap = typeMap;
        }

        public bool Matches(ConstruktionContext context)
        {
            var typeInfo = context.RequestType.GetTypeInfo();

            return (typeInfo.IsInterface &&
                    _typeMap.ContainsKey(context.RequestType))
                   ||
                   (!typeInfo.IsGenericType &&
                    typeInfo.IsClass &&
                    context.RequestType.HasNonDefaultCtor());
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var implementation = _typeMap.ContainsKey(context.RequestType)
                ? _typeMap[context.RequestType]
                : context.RequestType;

            var ctor = BuildCtor(implementation, pipeline);

            var instance = construct(ctor, pipeline);

            return instance;
        }

        private Func<object> BuildCtor(Type type, ConstruktionPipeline pipeline)
        {
            var greedyCtor = type.GetTypeInfo()
                .DeclaredConstructors
                .ToList()
                .Greediest();

            var @params = new List<ConstantExpression>();
            foreach (var parameter in greedyCtor.GetParameters())
            {
                var ctorArg = parameter.ParameterType;

                var value = pipeline.Construct(new ConstruktionContext(ctorArg));

                @params.Add(Expression.Constant(value));
            }

            var ctor = Expression.Lambda<Func<object>>(Expression.New(greedyCtor, @params)).Compile();

            return ctor;
        }

        private object construct(Func<object> ctor, ConstruktionPipeline pipeline)
        {
            var instance = ctor();

            var properties = instance.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.CanWrite);

            foreach (var property in properties)
            {
                var result = pipeline.Construct(new ConstruktionContext(property));

                property.SetValue(instance, result);
            }

            return instance;
        }
    }
}