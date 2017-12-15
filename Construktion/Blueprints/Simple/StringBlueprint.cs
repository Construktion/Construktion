namespace Construktion.Blueprints.Simple
{
    public class StringBlueprint : AbstractBlueprint<string>
    {
        public override string Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var prefix = context.PropertyInfo == null ? "String" : context.PropertyInfo.Name;

            var result = prefix + "-" + _random.Next(1, 10000);

            return result;
        }
    }
}