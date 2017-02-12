namespace Construktion.Blueprints
{
    public interface Blueprint 
    {
        bool Matches(ConstruktionContext context);
        //need to refactor out the pipeline
        object Build(ConstruktionContext context, ConstruktionPipeline pipeline);
    }
}
