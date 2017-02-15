namespace Construktion
{
    using System;
    using System.Reflection;

    public class ConstruktionContext
    {
        public Type RequestType { get; }
        public PropertyInfo PropertyInfo { get; }

        public ConstruktionContext(Type request)
        {
            request.ThrowIfNull(nameof(request));

            RequestType = request;
        }

        public ConstruktionContext(PropertyInfo propertyInfo)
        {
            propertyInfo.ThrowIfNull(nameof(propertyInfo));

            RequestType = propertyInfo.PropertyType;
            PropertyInfo = propertyInfo;
        }
    }
}