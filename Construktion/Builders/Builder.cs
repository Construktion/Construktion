namespace Construktion.Builders
{
    public interface Builder 
    {
        bool CanBuild(RequestContext context);
        object Build(RequestContext context, ConstruktionPipeline pipeline);
    }
}
