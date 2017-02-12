namespace Construktion.Blueprints
{
    using System;

    public class GuidBlueprint : Blueprint
    {
        public bool Matches(ConstruktionContext context)
        {
            return context.RequestType == typeof(Guid);
        }

        public object Build(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            return Guid.NewGuid();
        }
    }
}
