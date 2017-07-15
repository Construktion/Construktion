namespace Construktion.Blueprints.Simple
{
    using System;
    using System.Collections.Generic;

    public class InterfaceBlueprint : Blueprint
    {
        private readonly Dictionary<Type, Type> _typeMap;

        public InterfaceBlueprint() : this (new Dictionary<Type, Type>())
        {

        }

        public InterfaceBlueprint(Dictionary<Type, Type> typeMap)
        {
            _typeMap = typeMap;
        }

        public bool Matches(ConstruktionContext context)
        {          
            return _typeMap.ContainsKey(context.RequestType);
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var implementation = _typeMap[context.RequestType];

            var result = pipeline.Send(new ConstruktionContext(implementation));
            
            return result;
        }        
    }
}