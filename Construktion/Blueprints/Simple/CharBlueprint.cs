namespace Construktion.Blueprints.Simple
{    
    public class CharBlueprint : AbstractBlueprint<char>
    {    
        public override object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var chars = "$%#@!*abcdefghijklmnopqrstuvwxyz1234567890?;:ABCDEFGHIJKLMNOPQRSTUVWXYZ^&";
            var num = _random.Next(0, chars.Length);

            return chars[num];
        }
    }
}
