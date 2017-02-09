namespace Construktion
{
    using System;
    using System.Reflection;

    public class ConstruktionContext
    {
        public Type RequestType { get; }
        public Maybe<Type> ParentClass { get; set; }
        public Maybe<PropertyInfo> PropertyInfo { get; }

        public ConstruktionContext(Type request)
        {
            request.ThrowIfNull(nameof(request));

            RequestType = request;
            PropertyInfo = Maybe.Empty<PropertyInfo>();
            ParentClass =Maybe.Empty<Type>();
        }

        public ConstruktionContext(PropertyInfo propertyInfo)
        {
            propertyInfo.ThrowIfNull(nameof(propertyInfo));

            RequestType = propertyInfo.PropertyType;
            PropertyInfo = new Maybe<PropertyInfo>(propertyInfo);
            ParentClass = new Maybe<Type>(propertyInfo.DeclaringType);
        }
    }
}