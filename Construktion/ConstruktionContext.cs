namespace Construktion
{
    using System;
    using System.Reflection;

    /// <summary>
    /// The context of the type currently being constructed
    /// </summary>
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
            request.GuardNull();

            RequestType = request;
        }

        public ConstruktionContext(PropertyInfo propertyInfo)
        {
            propertyInfo.GuardNull();

            PropertyInfo = propertyInfo;
            RequestType = propertyInfo.PropertyType;
        }

        public ConstruktionContext(ParameterInfo parameterInfo)
        {
            parameterInfo.GuardNull();

            ParameterInfo = parameterInfo;
            RequestType = parameterInfo.ParameterType;
        }
    }
}