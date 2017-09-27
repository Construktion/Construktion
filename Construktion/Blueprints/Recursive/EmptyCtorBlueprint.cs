using System.Reflection;

namespace Construktion.Blueprints.Recursive
{
    public class EmptyCtorBlueprint : Blueprint
    {
        public bool Matches(ConstruktionContext context)
        {
            return context.RequestType.GetTypeInfo().IsClass &&
                   context.RequestType.HasDefaultCtor();
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var instance = context.RequestType.NewUp();

            var properties = pipeline.Settings.PropertyStrategy(context.RequestType);

            foreach (var property in properties)
            {
                var result = pipeline.Send(new ConstruktionContext(property));

                property.SetPropertyValue(instance, result);
            }

            return instance;
        }
    }
}
