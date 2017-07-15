namespace Construktion.Blueprints.Simple
{
    public class IgnoreVirtualPropertiesBlueprint : Blueprint
    {
        public bool Matches(ConstruktionContext context)
        {
            return context.PropertyInfo?.GetGetMethod().IsVirtual ?? false;
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            return default(object);
        }
    }
}
