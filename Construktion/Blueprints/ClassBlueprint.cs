namespace Construktion.Blueprints
{
    using System;
    using System.Linq;
    using System.Reflection;

    public class ClassBlueprint : Blueprint
    {
        public bool Matches(ConstruktionContext context)
        {
            return context.RequestType.GetTypeInfo().IsClass && context.RequestType.HasDefaultCtor();
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var instance = Activator.CreateInstance(context.RequestType);

            var properties = context.RequestType.GetRuntimeProperties().ToList();
                //.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var result = pipeline.Construct(new ConstruktionContext(property));
                    
                property.SetValue(instance, result);
            }

            return instance;
        }
    }
}
