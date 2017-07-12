namespace Construktion.Blueprints.Simple
{
    public class BoolBlueprint : AbstractBlueprint<bool>
    {
        private bool value = false;

        public override object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            value = !value;

            return value;
        }
    }
}