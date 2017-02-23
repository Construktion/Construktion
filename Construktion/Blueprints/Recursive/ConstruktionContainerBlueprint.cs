namespace Construktion.Blueprints.Recursive
{
    using System.Linq;
    using System.Reflection;

    public class ConstruktionContainerBlueprint : Blueprint
    {
        private readonly ConstruktionContainer _container;

        public ConstruktionContainerBlueprint(ConstruktionContainer container)
        {
            _container = container;
        }

        public bool Matches(ConstruktionContext context)
        {
            try
            {
                return _container.GetInstance(context.RequestType) != null;
            }
            catch
            {
                return false;
            }
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var instance = _container.GetInstance(context.RequestType);

            var properties = context.RequestType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.CanWrite);

            foreach (var property in properties)
            {
                var result = pipeline.Construct(new ConstruktionContext(property));

                property.SetValue(instance, result);
            }

            return instance;
        }
    }
}