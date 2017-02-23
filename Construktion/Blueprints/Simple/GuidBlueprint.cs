namespace Construktion.Blueprints
{
    using System;

    public class GuidBlueprint : Blueprint
    {
        public bool Matches(ConstruktionContext context)
        {
            return context.Request == typeof(Guid);
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            return Guid.NewGuid();
        }
    }
}
