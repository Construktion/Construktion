namespace Construktion.Blueprints.Recursive
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Internal;

    public class NonEmptyCtorBlueprint : Blueprint
    {
        public bool Matches(ConstruktionContext context)
        {
            var typeInfo = context.RequestType.GetTypeInfo();

            return context.RequestType.HasNonDefaultCtor() &&
                   !typeInfo.IsGenericType &&
                   typeInfo.IsClass;
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var instance = NewUp(context.RequestType, pipeline);

            var result = construct(instance, pipeline);

            return result;
        }

        private object NewUp(Type type, ConstruktionPipeline pipeline)
        {
            var ctors = type.GetTypeInfo()
                            .DeclaredConstructors
                            .ToList();

            var ctor = pipeline.Settings.CtorStrategy(ctors);

            var @params = new List<object>();
            foreach (var parameter in ctor.GetParameters())
            {
                var ctorArg = parameter.ParameterType;

                var value = pipeline.Send(new ConstruktionContext(ctorArg));

                @params.Add(value);
            }

            return type.NewUp(@params.ToArray());
        }

        private object construct(object instance, ConstruktionPipeline pipeline)
        {
            var properties = pipeline.Settings.PropertyStrategy(instance.GetType());

            foreach (var property in properties)
            {
                var result = pipeline.Send(new ConstruktionContext(property));

                property.SetPropertyValue(instance, result);
            }

            return instance;
        }
    }
}