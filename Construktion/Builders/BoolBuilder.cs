namespace Construktion.Builders
{
    public class BoolBuilder : Builder
    {
        private bool value;

        public bool Matches(ConstruktionContext context)
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
