namespace Construktion
{
    using System;
    using System.Reflection;

    public class BuildContext
    {
        public Type RequestType { get; }

        //separate interfaces?
        public Maybe<Type> ParentClass { get; set; }
        public Maybe<PropertyInfo> PropertyInfo { get; }

        public BuildContext(Type request)
        {
            request.ThrowIfNull(nameof(request));

            RequestType = request;
            PropertyInfo = Maybe.Empty<PropertyInfo>();
            ParentClass = Maybe.Empty<Type>();
        }

        public BuildContext(PropertyInfo propertyInfo)
        {
            propertyInfo.ThrowIfNull(nameof(propertyInfo));

            RequestType = propertyInfo.PropertyType;
            PropertyInfo = new Maybe<PropertyInfo>(propertyInfo);
            ParentClass = new Maybe<Type>(propertyInfo.DeclaringType);
        }
    }
}