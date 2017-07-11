namespace Construktion.Blueprints.Simple
{
    using System;
    using Blueprints;

    public class ScopedBlueprint : Blueprint
    {
        private readonly Type _type;
        private readonly object _instance;
       
        public ScopedBlueprint(Type type, object instance)
        {
            _type = type;
            _instance = instance;
        }

        public bool Matches(ConstruktionContext context)
        {
            return _type == context.RequestType;
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            return _instance;
        }
    }
}
