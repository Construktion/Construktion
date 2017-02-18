namespace Construktion
{
    using System;
    using System.Reflection;

    public class ConstruktionContext
    {
        public Type Request { get; }
        public PropertyContext PropertyContext { get; }

        public ConstruktionContext(Type request)
        {
            request.ThrowIfNull(nameof(request));

            Request = request;
            PropertyContext = new PropertyContext();
        }

        public ConstruktionContext(PropertyInfo propertyInfo)
        {
            propertyInfo.ThrowIfNull(nameof(propertyInfo));

            Request = propertyInfo.PropertyType;
            PropertyContext = new PropertyContext(propertyInfo);
        }
    }
}