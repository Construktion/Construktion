namespace Construktion.Blueprints
{
    using System;
    using System.Linq;
    using System.Reflection;

    public abstract class AbstractAttributeBlueprint<T> : Blueprint where T : Attribute
    {
        protected T GetAttribute(ConstruktionContext context)
        {
            return (T) context.PropertyInfo.GetCustomAttribute(typeof(T));
        }

        public virtual bool Matches(ConstruktionContext context)
        {
            return context.PropertyInfo?.GetCustomAttributes(typeof(T))
                .ToList()
                .Any() ?? false;
        }

        public abstract object Construct(ConstruktionContext context, ConstruktionPipeline pipeline);
    }
}
