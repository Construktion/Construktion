namespace Construktion.Blueprints
{
    //todo a lot of the simple blueprints can be converted to use this base class 
    public abstract class AbstractBlueprint<T> : Blueprint
    {
        public virtual bool Matches(ConstruktionContext context)
        {
            return context.RequestType == typeof(T);
        }

        public abstract object Construct(ConstruktionContext context, ConstruktionPipeline pipeline);
    }
}