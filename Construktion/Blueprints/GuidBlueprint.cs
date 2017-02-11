namespace Construktion.Blueprints
{
    using System;

    public class GuidBlueprint : Blueprint
    {
        public bool Matches(BuildContext context)
        {
            return context.RequestType == typeof(Guid);
        }

        public object Build(BuildContext context, ConstruktionPipeline pipeline)
        {
            return Guid.NewGuid();
        }
    }
}
