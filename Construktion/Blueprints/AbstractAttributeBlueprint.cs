namespace Construktion.Blueprints
{
    using System;
    using System.Linq;
    using System.Reflection;

    public abstract class AbstractAttributeBlueprint<T> : Blueprint where T : Attribute
    {
        protected T GetAttribute(ConstruktionContext context)
        {
           return (T)context.PropertyInfo
                .GetCustomAttributes(typeof(T))
                .First();
        }

        public bool Matches(ConstruktionContext context)
        {
            return context.HasAttribute<T>();
        }

        public abstract object Construct(ConstruktionContext context, ConstruktionPipeline pipeline);
    }
}