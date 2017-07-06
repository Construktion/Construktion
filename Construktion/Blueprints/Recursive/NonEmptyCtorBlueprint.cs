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
        private readonly Func<List<ConstructorInfo>, ConstructorInfo> _ctorStrategy;
        private readonly Func<Type, IEnumerable<PropertyInfo>> _propertiesSelector;

        public NonEmptyCtorBlueprint(Dictionary<Type, Type> typeMap) : this(typeMap, Extensions.GreedyCtor, Extensions.PropertiesWithPublicSetter)
        {
            
        }

        public NonEmptyCtorBlueprint(Dictionary<Type, Type> typeMap, Func<List<ConstructorInfo>, ConstructorInfo> ctorStrategy, Func<Type, IEnumerable<PropertyInfo>> propertiesSelector)
        {
            _typeMap = typeMap;
            _ctorStrategy = ctorStrategy;
            _propertiesSelector = propertiesSelector;
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

            var instance = construct(ctor(), pipeline);

            return instance;
        }

        private Func<object> BuildCtor(Type type, ConstruktionPipeline pipeline)
        {
            var ctors = type.GetTypeInfo()
                .DeclaredConstructors
                .ToList();

            var ctor = _ctorStrategy(ctors);

            var @params = new List<ConstantExpression>();
            foreach (var parameter in ctor.GetParameters())
            {
                var ctorArg = parameter.ParameterType;

                var value = pipeline.Construct(new ConstruktionContext(ctorArg));

                @params.Add(Expression.Constant(value));
            }

            return Expression.Lambda<Func<object>>(Expression.New(ctor, @params)).Compile();
        }

        private object construct(object instance, ConstruktionPipeline pipeline)
        {
            var properties = _propertiesSelector(instance.GetType());

            foreach (var property in properties)
            {
                var result = pipeline.Construct(new ConstruktionContext(property));

                property.SetValue(instance, result);
            }

            return instance;
        }
    }
}