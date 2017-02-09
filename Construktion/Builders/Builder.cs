namespace Construktion.Builders
{
    public interface Builder 
    {
        bool CanBuild(ConstruktionContext context);
        object Build(ConstruktionContext context, ConstruktionPipeline pipeline);
    }
}
