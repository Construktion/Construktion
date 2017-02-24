namespace Construktion.Blueprints.Recursive
{
    using System;
    using System.Linq;
    using System.Reflection;

    public class EmptyCtorBlueprint : Blueprint
    {
        public bool Matches(ConstruktionContext context)
        {
            //need to see what happens for Class<TClass>
            return context.RequestType.GetTypeInfo().IsClass &&
                   context.RequestType.HasDefaultCtor();
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var instance = Activator.CreateInstance(context.RequestType);

            var properties = context.RequestType
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
