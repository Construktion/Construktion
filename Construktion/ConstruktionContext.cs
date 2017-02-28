namespace Construktion
{
    using System;
    using System.Reflection;

    public class ConstruktionContext
    {
        /// <summary>
        /// The type of the current request
        /// </summary>
        public Type RequestType { get; }

        /// <summary>
        /// Non null, will be populated when a Property is requested
        /// </summary>
        public PropertyContext PropertyContext { get; } = new PropertyContext();

        ///<summary>
        ///Non null, will be populated when a ParameterInfo is requested
        ///</summary>
        public ParameterContext ParameterContext { get; } = new ParameterContext();

        public ConstruktionContext(Type request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            RequestType = request;
        }

        public ConstruktionContext(PropertyInfo propertyInfo)
            : this(propertyInfo?.PropertyType)
        {
            PropertyContext = new PropertyContext(propertyInfo);
        }

        public ConstruktionContext(ParameterInfo parameterInfo)
            : this(parameterInfo?.ParameterType)
        {
            ParameterContext = new ParameterContext(parameterInfo);
        }
    }
}