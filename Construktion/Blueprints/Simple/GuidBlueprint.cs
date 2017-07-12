namespace Construktion.Blueprints.Simple
{
    using System;

    public class GuidBlueprint : AbstractBlueprint<Guid>
    {
        public override object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            return Guid.NewGuid();
        }
    }
}
