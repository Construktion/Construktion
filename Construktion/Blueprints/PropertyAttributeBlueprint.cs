namespace Construktion.Blueprints
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Internal;

    /// <summary>
    /// Base class to construct properties from an attribute.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PropertyAttributeBlueprint<T> : Blueprint where T : Attribute
    {
        protected readonly Func<T, object> _value;

        public PropertyAttributeBlueprint(Func<T, object> value)
        {
            value.GuardNull();

            _value = value;
        }

        /// <summary>
        /// The base implementation returns true if the property has the attribute.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual bool Matches(ConstruktionContext context)
        {
            return context.PropertyInfo?.GetCustomAttributes(typeof(T))
                          .ToList()
                          .Any() ?? false;
        }

        /// <summary>
        /// Construct a property using its attribute value.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="pipeline"></param>
        /// <returns></returns>
        public virtual object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var attribute = (T)context.PropertyInfo.GetCustomAttribute(typeof(T));

            var value = _value(attribute);

            return value;
        }
    }
}