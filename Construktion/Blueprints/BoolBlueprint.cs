namespace Construktion.Blueprints
{
    public class BoolBlueprint : Blueprint
    {
        private bool value = false;

        public bool Matches(BuildContext context)
        {
            return context.RequestType == typeof(bool);
        }

        public object Build(BuildContext context, ConstruktionPipeline pipeline)
        {
            value = !value;

            return value;
        }
    }
}