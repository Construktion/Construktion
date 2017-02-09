namespace Construktion.Blueprints
{
    using System;
    using System.Reflection;

    public class ClassBlueprint : Blueprint
    {
        public bool Matches(ConstruktionContext context)
        {
            return context.RequestType.GetTypeInfo().IsClass;
        }

        public object Build(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var instance = Activator.CreateInstance(context.RequestType);

            var properties = context.RequestType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var pi = context.RequestType.GetProperty(property.Name);

                var result = pipeline.Build(new ConstruktionContext(pi));
                    
                pi.SetValue(instance, result);
            }

            return instance;
        }
    }
}
