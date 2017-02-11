namespace Construktion.Blueprints
{
    public interface Blueprint 
    {
        bool Matches(BuildContext context);
        object Build(BuildContext context, ConstruktionPipeline pipeline);
    }
}
