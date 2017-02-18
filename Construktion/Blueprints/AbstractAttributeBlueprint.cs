namespace Construktion.Blueprints
{
    using System;
    using System.Linq;

    public abstract class AbstractAttributeBlueprint<T> : Blueprint where T : Attribute
    {
        protected T GetAttribute(ConstruktionContext context)
        {
            return (T) context.PropertyContext.GetAttributes(typeof(T)).First();
        }

        public bool Matches(ConstruktionContext context)
        {
            return context.PropertyContext.GetAttributes(typeof(T))
                        .ToList()
                        .Any();
        }

        public abstract object Construct(ConstruktionContext context, ConstruktionPipeline pipeline);
    }
}