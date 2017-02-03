namespace Construktion.Builders
{
    using System;
    using System.Reflection;

    public class ClassBuilder : Builder
    {
        public bool CanBuild(Type request)
        {
            return request.GetTypeInfo().IsClass;
        }

        public object Build(RequestContext context)
        {
            var instance = Activator.CreateInstance(context.Request);

            var properties = context.Request.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var pi = context.Request.GetProperty(property.Name);

                var result = context.Construktion.Build(pi.PropertyType);

                pi.SetValue(instance, result);
            }

            return instance;
        }
    }
}
