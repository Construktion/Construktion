namespace Construktion.Blueprints.Simple
{
    public class BoolBlueprint : Blueprint
    {
        private bool value = false;

        public bool Matches(ConstruktionContext context)
        {
            return context.RequestType == typeof(bool);
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            value = !value;

            return value;
        }
    }
}