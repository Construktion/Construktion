namespace Construktion.Blueprints
{
    public interface Blueprint 
    {
        bool Matches(ConstruktionContext context);
        object Construct(ConstruktionContext context, ConstruktionPipeline pipeline);
    }
}
