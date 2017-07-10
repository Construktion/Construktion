namespace Construktion.Blueprints.Simple
{
    using System;
    using System.Reflection;
    using Blueprints;

    public class OmitPropertyBlueprint : Blueprint
    {
        private readonly Func<string, bool> _convention;
        private readonly Type _propertyType;

        public OmitPropertyBlueprint(Func<string, bool> convention, Type propertyType)
        {
            if (convention == null)
                throw new ArgumentNullException(nameof(convention));
            if (propertyType == null)
                throw new ArgumentNullException(nameof(propertyType));

            _convention = convention;
            _propertyType = propertyType;
        }

        public bool Matches(ConstruktionContext context)
        {
            return _convention(context.PropertyContext.Name) &&
                   context.RequestType == _propertyType;
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            return context.RequestType.GetTypeInfo().IsValueType 
                ? Activator.CreateInstance(context.RequestType) 
                : null;
        }
    }
}