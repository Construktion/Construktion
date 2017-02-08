namespace Construktion.Builders
{
    using System;
    using System.Linq;
    using System.Reflection;

    public abstract class AbstractAttributeBuilder<T> : Builder where T : Attribute
    {
        public T GetAttribute(RequestContext context)
        {
           return (T)context.PropertyInfo
                .Single()
                .GetCustomAttributes(typeof(T))
                .First();
        }

        public virtual bool CanBuild(RequestContext context)
        {
            return context.HasAttribute<T>();
        }

        public abstract object Build(RequestContext context, ConstruktionPipeline pipeline);
    }
}