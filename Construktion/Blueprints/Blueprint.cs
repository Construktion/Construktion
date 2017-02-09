namespace Construktion.Blueprints
{
    public interface Blueprint 
    {
        bool Matches(ConstruktionContext context);
        object Build(ConstruktionContext context, ConstruktionPipeline pipeline);
    }
}
