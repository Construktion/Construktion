namespace Construktion.Blueprints.Recursive
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public class NonEmptyCtorBlueprint : Blueprint
    {
        private readonly Func<List<ConstructorInfo>, ConstructorInfo> _ctorStrategy;
        private readonly Func<Type, IEnumerable<PropertyInfo>> _propertiesSelector;

        public NonEmptyCtorBlueprint()
            : this (Extensions.GreedyCtor, Extensions.PropertiesWithPublicSetter)
        {

        }

        public NonEmptyCtorBlueprint(Func<List<ConstructorInfo>, ConstructorInfo> ctorStrategy, Func<Type, IEnumerable<PropertyInfo>> propertiesSelector)
        {
            _ctorStrategy = ctorStrategy;
            _propertiesSelector = propertiesSelector;
        }

        public bool Matches(ConstruktionContext context)
        {
            var typeInfo = context.RequestType.GetTypeInfo();

            return context.RequestType.HasNonDefaultCtor() &&
                   !typeInfo.IsGenericType &&
                   typeInfo.IsClass;
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {         
            var ctor = BuildCtor(context.RequestType, pipeline);

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

                var value = pipeline.Send(new ConstruktionContext(ctorArg));

                @params.Add(Expression.Constant(value));
            }

            return Expression.Lambda<Func<object>>(Expression.New(ctor, @params)).Compile();
        }

        private object construct(object instance, ConstruktionPipeline pipeline)
        {
            var properties = _propertiesSelector(instance.GetType());

            foreach (var property in properties)
            {
                var result = pipeline.Send(new ConstruktionContext(property));

                property.SetValue(instance, result);
            }

            return instance;
        }
    }
}