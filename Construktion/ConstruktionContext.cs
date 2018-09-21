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
        /// The PropertyInfo being constructed. If the context does not represent a PropertyInfo,
        /// a stand in "Nullo" object will be used. 
        /// </summary>
        public PropertyInfo PropertyInfo { get; } = Extensions.NulloPropertyInfo;

        /// <summary>
        /// The ParameterInfo being constructed. If the context does not represent a ParameterInfo,
        /// a stand in "Nullo" object will be used. 
        /// </summary>
        public ParameterInfo ParameterInfo { get; } = Extensions.NulloParameterInfo;

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