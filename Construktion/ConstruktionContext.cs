namespace Construktion
{
    using System;
    using System.Reflection;

    public class ConstruktionContext
    {
        public Type RequestType { get; }
        public PropertyContext PropertyContext { get; }

        public ConstruktionContext(Type request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            RequestType = request;
            PropertyContext = new PropertyContext();
        }

        public ConstruktionContext(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException(nameof(propertyInfo));

            RequestType = propertyInfo.PropertyType;
            PropertyContext = new PropertyContext(propertyInfo);
        }
    }
}