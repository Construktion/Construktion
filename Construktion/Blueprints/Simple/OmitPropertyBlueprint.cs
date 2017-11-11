namespace Construktion.Blueprints.Simple
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Internal;

    public class OmitPropertyBlueprint : Blueprint
    {
        private readonly Func<PropertyInfo, bool> _convention;
        private readonly IEnumerable<Type> _propertyTypes;

        public OmitPropertyBlueprint(Func<PropertyInfo, bool> convention, Type propertyType) : this(convention,
            new List<Type> { propertyType }) { }

        public OmitPropertyBlueprint(Func<PropertyInfo, bool> convention, IEnumerable<Type> propertyTypes)
        {
            convention.GuardNull();
            propertyTypes.GuardNull();

            _convention = convention;
            _propertyTypes = propertyTypes;
        }

        public bool Matches(ConstruktionContext context)
        {
            var matchesType = _propertyTypes.Contains(context.RequestType) ||
                              ContainsGeneric(context.RequestType) ||
                              !_propertyTypes.Any();

            return context.PropertyInfo != null && _convention(context.PropertyInfo) && matchesType;
        }

        private bool ContainsGeneric(Type requestType)
        {
            var typeInfo = requestType.GetTypeInfo();

            return typeInfo.IsGenericType && _propertyTypes.Contains(typeInfo.GetGenericTypeDefinition());
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            return context.RequestType.GetTypeInfo().IsValueType
                       ? Activator.CreateInstance(context.RequestType)
                       : null;
        }
    }
}