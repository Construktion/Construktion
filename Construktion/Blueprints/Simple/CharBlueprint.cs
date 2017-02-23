namespace Construktion.Blueprints.Simple
{
    using System;

    public class CharBlueprint : Blueprint
    {
        private readonly Random _random = new Random();

        public bool Matches(ConstruktionContext context)
        {
            return context.RequestType == typeof(char);
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var chars = "$%#@!*abcdefghijklmnopqrstuvwxyz1234567890?;:ABCDEFGHIJKLMNOPQRSTUVWXYZ^&";
            var num = _random.Next(0, chars.Length);

            return chars[num];
        }
    }
}
