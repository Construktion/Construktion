namespace Construktion.Blueprints.Simple
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Blueprints;

    public class OmitPropertyBlueprint : Blueprint
    {
        private readonly Func<string, bool> _convention;
        private readonly List<Type> _propertyTypes;

        public OmitPropertyBlueprint(Func<string, bool> convention, Type propertyType) : this(convention, new List<Type> { propertyType})
        {
        }

        public OmitPropertyBlueprint(Func<string, bool> convention, List<Type> propertyTypes)
        {
            if (convention == null)
                throw new ArgumentNullException(nameof(convention));
            if (propertyTypes == null)
                throw new ArgumentNullException(nameof(propertyTypes));

            _convention = convention;
            _propertyTypes = propertyTypes;
        }

        public bool Matches(ConstruktionContext context)
        {
            return _convention(context.PropertyContext.Name) &&
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