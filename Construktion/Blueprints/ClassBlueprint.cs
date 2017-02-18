namespace Construktion.Blueprints
{
    using System;
    using System.Linq;
    using System.Reflection;

    public class ClassBlueprint : Blueprint
    {
        public bool Matches(ConstruktionContext context)
        {
            return context.Request.GetTypeInfo().IsClass && context.Request.HasDefaultCtor();
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var instance = Activator.CreateInstance(context.Request);

            var properties = context.Request.GetRuntimeProperties().ToList();
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
