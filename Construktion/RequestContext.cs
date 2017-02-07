namespace Construktion
{
    using System;
    using System.Reflection;

    public class RequestContext
    {
        public Type RequestType { get; }
        public PropertyInfo PropertyInfo { get; }

        public RequestContext(Type request)
        {
            RequestType = request;
        }

        public RequestContext(PropertyInfo propertyInfo)
        {
            RequestType = propertyInfo.PropertyType;
            PropertyInfo = propertyInfo;
        }
    }
}