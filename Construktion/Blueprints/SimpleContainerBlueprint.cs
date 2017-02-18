namespace Construktion.Blueprints
{
    public class SimpleContainerBlueprint : Blueprint
    {
        private readonly SimpleContainer _container;

        public SimpleContainerBlueprint(SimpleContainer container)
        {
            _container = container;
        }

        public bool Matches(ConstruktionContext context)
        {
            try
            {
                return _container.GetInstance(context.Request) != null;
            }
            catch
            {
                return false;
            }
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            return _container.GetInstance(context.Request);
        }
    }
}