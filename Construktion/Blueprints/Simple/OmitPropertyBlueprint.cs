namespace Construktion.Blueprints.Simple
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Blueprints;

    public class OmitPropertyBlueprint : Blueprint
    {
        private readonly Func<string, bool> _convention;
        private readonly IEnumerable<Type> _propertyTypes;

        public OmitPropertyBlueprint(Func<string, bool> convention, Type propertyType) : this(convention, new List<Type> { propertyType})
        {
        }

        public OmitPropertyBlueprint(Func<string, bool> convention, IEnumerable<Type> propertyTypes)
        {
            convention.GuardNull();
            propertyTypes.GuardNull();

            _convention = convention;
            _propertyTypes = propertyTypes;
        }

        public bool Matches(ConstruktionContext context)
        {
            return
                context.PropertyInfo != null &&
                _convention(context.PropertyInfo.Name) &&
                _propertyTypes.Contains(context.RequestType);
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            return context.RequestType.GetTypeInfo().IsValueType 
                ? Activator.CreateInstance(context.RequestType) 
                : null;
        }
    }
}