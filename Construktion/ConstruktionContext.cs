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
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            Request = request;
            PropertyContext = new PropertyContext();
        }

        public ConstruktionContext(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException(nameof(propertyInfo));

            Request = propertyInfo.PropertyType;
            PropertyContext = new PropertyContext(propertyInfo);
        }
    }
}