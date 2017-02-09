namespace Construktion.Builders
{
    using System;
    using System.Linq;
    using System.Reflection;

    public abstract class AbstractAttributeBuilder<T> : Builder where T : Attribute
    {
        public T GetAttribute(ConstruktionContext context)
        {
           return (T)context.PropertyInfo
                .Single()
                .GetCustomAttributes(typeof(T))
                .First();
        }

        public virtual bool Matches(ConstruktionContext context)
        {
            return context.HasAttribute<T>();
        }

        public abstract object Build(ConstruktionContext context, ConstruktionPipeline pipeline);
    }
}