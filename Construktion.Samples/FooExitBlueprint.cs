namespace Construktion.Samples
{
    public class FooExitBlueprint : AbstractExitBlueprint<Foo>
    {
        public override Foo Construct(Foo item, ConstruktionPipeline pipeline)
        {
            item.Name = "Ping";

            return item;
        }
    }
}