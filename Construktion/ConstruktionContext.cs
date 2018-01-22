namespace Construktion
{
    using System;
    using System.Reflection;
    using Internal;

    public class ConstruktionContext
    {
        /// <summary>
        /// The type of the current request
        /// </summary>
        public Type RequestType { get; }

        /// <summary>
        /// When not null PropertyInfo is being constructed
        /// </summary>
        public PropertyInfo PropertyInfo { get; }

        ///<summary>
        ///When not null ParameterInfo is being constructed
        ///</summary>
        public ParameterInfo ParameterInfo { get; }

        public ConstruktionContext(Type request)
        {
            request = request ?? throw new ArgumentNullException(nameof(request));

            RequestType = request;
        }

        public ConstruktionContext(PropertyInfo propertyInfo)
        {
            propertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));

            PropertyInfo = propertyInfo;
            RequestType = propertyInfo.PropertyType;
        }

        public ConstruktionContext(ParameterInfo parameterInfo)
        {
            parameterInfo = parameterInfo ?? throw new ArgumentNullException(nameof(parameterInfo));

            ParameterInfo = parameterInfo;
            RequestType = parameterInfo.ParameterType;
        }
    }
}