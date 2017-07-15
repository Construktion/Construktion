namespace Construktion.Blueprints
{
    using System;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Base class to construct properties from an attribute
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

        public virtual bool Matches(ConstruktionContext context)
        {
            return context.PropertyInfo?.GetCustomAttributes(typeof(T))
                       .ToList()
                       .Any() ?? false;
        }

        public virtual object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var attribute = (T)context.PropertyInfo.GetCustomAttribute(typeof(T));

            var value = _value(attribute);

            return value;
        }
    }
}