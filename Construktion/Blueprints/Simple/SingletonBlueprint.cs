namespace Construktion.Blueprints.Simple
{
    using System;

    public class SingletonBlueprint : Blueprint
    {
        private readonly Type _type;
        private readonly object _instance;

        public SingletonBlueprint(Type type, object instance)
        {
            _type = type;
            _instance = instance;
        }

        public bool Matches(ConstruktionContext context) => _type == context.RequestType;

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline) => _instance;
    }
}