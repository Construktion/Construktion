namespace Construktion.Blueprints.Simple
{
    using System;
    using System.Reflection;
    using Blueprints;

    public class OmitIdBlueprint : Blueprint
    {
        private readonly Func<string, bool> _convention;
        private readonly Type _idType;

        public OmitIdBlueprint() 
        : this(x => x.EndsWith("Id"), typeof(int))
        { 
            
        }

        public OmitIdBlueprint(Func<string, bool> convention, Type idType)
        {
            _convention = convention;
            _idType = idType;
        }

        public bool Matches(ConstruktionContext context)
        {
            return _convention(context.PropertyContext.Name) &&
                   context.RequestType == _idType;
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            return context.RequestType.GetTypeInfo().IsValueType 
                ? Activator.CreateInstance(context.RequestType) 
                : null;
        }
    }
}