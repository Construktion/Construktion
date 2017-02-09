namespace Construktion.Builders
{
    public class BoolBuilder : Builder
    {
        private bool value;

        public bool CanBuild(ConstruktionContext context)
        {
            return context.RequestType == typeof(bool);
        }

        public object Build(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            value = !value;

            return value;
        }
    }
}
