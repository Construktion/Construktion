namespace Construktion.Blueprints.Simple
{
    internal class StringBlueprint : AbstractBlueprint<string>
    {
        public override string Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var result = "String-" + _random.Next(1, 10000);

            return result;
        }
    }
}