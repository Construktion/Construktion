namespace Construktion
{
    using System;
    using System.Reflection;

    public class RequestContext
    {
        public Type RequestType { get; }
        public Maybe<PropertyInfo> PropertyInfo { get; }

        public RequestContext(Type request)
        {
            request.ThrowIfNull(nameof(request));

            RequestType = request;
            PropertyInfo = Maybe.Empty<PropertyInfo>();
        }

        public RequestContext(PropertyInfo propertyInfo)
        {
            propertyInfo.ThrowIfNull(nameof(propertyInfo));

            RequestType = propertyInfo.PropertyType;
            PropertyInfo = propertyInfo.ToMaybe();
        }
    }
}