namespace Construktion
{ 
    public interface ExitBlueprint
    {
        bool Matches(object item, ConstruktionContext context);
        object Construct(object item, ConstruktionPipeline pipeline);
    }

    public interface ExitBlueprint<T>
    {
        bool Matches(T item, ConstruktionContext context);
        T Construct(T item, ConstruktionPipeline pipeline);
    }
}