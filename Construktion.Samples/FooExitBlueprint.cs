namespace Construktion.Samples
{
    using Blueprints;

    public class FooExitBlueprint : AbstractExitBlueprint<Foo>
    {
        public override Foo Construct(Foo item, ConstruktionPipeline pipeline)
        {
            item.Name = "Ping";

            return item;
        }
    }
}