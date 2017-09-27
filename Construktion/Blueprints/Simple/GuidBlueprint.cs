using System;

namespace Construktion.Blueprints.Simple
{
    public class GuidBlueprint : AbstractBlueprint<Guid>
    {
        public override Guid Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            return Guid.NewGuid();
        }
    }
}
