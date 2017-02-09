namespace Construktion.Builders
{
    public interface Builder 
    {
        bool Matches(ConstruktionContext context);
        object Build(ConstruktionContext context, ConstruktionPipeline pipeline);
    }
}
