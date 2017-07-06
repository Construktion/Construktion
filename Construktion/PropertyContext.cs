namespace Construktion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class PropertyContext
    {
        private readonly PropertyInfo _propertyInfo;

        /// <summary>
        /// The name of the property. An empty string indicates a PropertyContext is NOT being constructed.
        /// </summary>
        public string Name => _propertyInfo?.Name ?? "";

        public PropertyContext()
        {

        }

        public PropertyContext(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException(nameof(propertyInfo));

            _propertyInfo = propertyInfo;
        }

        public bool IsType(Type type) => _propertyInfo != null && _propertyInfo.PropertyType == type;

        /// <summary>
        /// Returns the attributes for a property. An empty list is returned when none are found.
        /// </summary>
        public IEnumerable<Attribute> GetAttributes(Type attr)
        {
            return (IEnumerable<Attribute>)_propertyInfo?.GetCustomAttributes(attr, false).ToList() ?? new List<Attribute>();
        }
    }
}