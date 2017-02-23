namespace Construktion.Blueprints
{
    using System;

    public class DefensiveBlueprint : Blueprint
    {
        public bool Matches(ConstruktionContext context)
        {
            return true;
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            throw new Exception($"No Blueprint could be found for {context.Request.FullName}");
        }
    }
}