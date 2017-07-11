namespace Construktion.Blueprints.Simple
{
    public class Blueprint<T> : AbstractBlueprint<T>
    {
        public override object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var result = pipeline.Construct(new ConstruktionContext(context.RequestType));

            return result;
        }
    }
}