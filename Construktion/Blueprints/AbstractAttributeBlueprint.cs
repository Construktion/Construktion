namespace Construktion.Blueprints
{
    using System;
    using System.Linq;
    using System.Reflection;

    public abstract class AbstractAttributeBlueprint<T> : Blueprint where T : Attribute
    {
        protected T Attribute(ConstruktionContext context)
        {
           return (T)context.PropertyInfo
                .Single()
                .GetCustomAttributes(typeof(T))
                .First();
        }

        public bool Matches(ConstruktionContext context)
        {
            return context.HasAttribute<T>() && AlsoMustMatch(context);
        }

        /// <summary>
        /// Allows additional criteria to be added to the blueprint.
        /// The base implementation always returns true;
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual bool AlsoMustMatch(ConstruktionContext context)
        {
            return true;
        } 

        public abstract object Build(ConstruktionContext context, ConstruktionPipeline pipeline);
    }
}