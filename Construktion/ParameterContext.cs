namespace Construktion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class ParameterContext
    {
        private readonly ParameterInfo _parameterInfo;

        /// <summary>
        /// The name of the parameter. An empty string indicates a parameter context is NOT being constructed.
        /// </summary>
        public string Name => _parameterInfo?.Name ?? "";

        public ParameterContext()
        {

        }

        public ParameterContext(ParameterInfo parameterInfo)
        {
            _parameterInfo = parameterInfo;
        }

        public bool IsType(Type type) => _parameterInfo != null && _parameterInfo.ParameterType == type;

        /// <summary>
        /// Returns the attributes for a parameter. An empty list is returned when none are found.
        /// </summary>
        public IEnumerable<Attribute> GetAttributes(Type attr)
        {
            return (IEnumerable<Attribute>)_parameterInfo?.GetCustomAttributes(attr, false).ToList() ?? new List<Attribute>();
        }
    }
}