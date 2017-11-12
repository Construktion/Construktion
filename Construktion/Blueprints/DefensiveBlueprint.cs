namespace Construktion.Blueprints
{
    using System;
    using System.Reflection;

    public class DefensiveBlueprint : Blueprint
    {
        public bool Matches(ConstruktionContext context) => true;

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            if (context.RequestType.GetTypeInfo().IsInterface)
                throw new Exception($"Cannot construct the interface {context.RequestType.Name}. " +
                                    "You must register it or add a custom blueprint.");


            throw new Exception($"No Blueprint could be found for {context.RequestType.FullName}. Please add " +
                                $"a custom blueprint that can create it.");
        }
    }
}