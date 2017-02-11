namespace Construktion.Blueprints
{
    using System;
    using System.Reflection;

    public class ClassBlueprint : Blueprint
    {
        public bool Matches(BuildContext context)
        {
            return context.RequestType.GetTypeInfo().IsClass;
        }

        public object Build(BuildContext context, ConstruktionPipeline pipeline)
        {
            var instance = Activator.CreateInstance(context.RequestType);

            var properties = context.RequestType.GetRuntimeProperties();
                
                //.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var pi = context.RequestType.GetRuntimeProperty(property.Name);

                var result = pipeline.Build(new BuildContext(pi));
                    
                pi.SetValue(instance, result);
            }

            return instance;
        }
    }
}
