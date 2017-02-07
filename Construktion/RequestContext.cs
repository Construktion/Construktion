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
            RequestType = request;
            PropertyInfo = new Maybe<PropertyInfo>();
        }

        public RequestContext(PropertyInfo propertyInfo)
        {
            RequestType = propertyInfo.PropertyType;
            PropertyInfo = propertyInfo.ToMaybe();
        }
    }
}