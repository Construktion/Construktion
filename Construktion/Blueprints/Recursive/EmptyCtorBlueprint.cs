namespace Construktion.Blueprints.Recursive
{
    using System;
    using System.Collections.Generic;    
    using System.Reflection;

    public class EmptyCtorBlueprint : Blueprint
    {
        private readonly Func<Type, IEnumerable<PropertyInfo>> _propertiesSelector;

        public EmptyCtorBlueprint() : this (Extensions.PropertiesWithPublicSetter)
        {

        }

        public EmptyCtorBlueprint(Func<Type, IEnumerable<PropertyInfo>> propertiesSelector)
        {
            _propertiesSelector = propertiesSelector;
        }

        public bool Matches(ConstruktionContext context)
        {
            return context.RequestType.GetTypeInfo().IsClass &&
                   context.RequestType.HasDefaultCtor();
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var instance = Activator.CreateInstance(context.RequestType);

            var properties = _propertiesSelector(context.RequestType);

            foreach (var property in properties)
            {
                var result = pipeline.Send(new ConstruktionContext(property));

                property.SetValue(instance, result);
            }

            return instance;
        }
    }
}
