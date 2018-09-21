namespace Construktion.Blueprints.Simple
{
    using Internal;

    public class StringBlueprint : AbstractBlueprint<string>
    {
        public override string Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var prefix = context.PropertyInfo.IsNulloPropertyInfo() ? "String" : context.PropertyInfo.Name;

            var result = prefix + "-" + _random.Next(1, 10000);

            return result;
        }
    }
}