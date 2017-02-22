namespace Construktion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class PropertyContext
    {
        private readonly PropertyInfo _propertyInfo;

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

        public IEnumerable<Attribute> GetAttributes(Type attr)
        {
            return _propertyInfo?.GetCustomAttributes(attr, false).ToList() ?? new List<Attribute>();
        }
    }
}