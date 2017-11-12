namespace Construktion.Blueprints.Simple
{
    using System;

    public class GuidBlueprint : AbstractBlueprint<Guid>
    {
        public override Guid Construct(ConstruktionContext context, ConstruktionPipeline pipeline) => Guid.NewGuid();
    }
}