namespace Construktion.Blueprints
{
    using System;

    public class CharBlueprint : Blueprint
    {
        private readonly Random _random = new Random();

        public bool Matches(BuildContext context)
        {
            return context.RequestType == typeof(char);
        }

        public object Build(BuildContext context, ConstruktionPipeline pipeline)
        {
            var chars = "$%#@!*abcdefghijklmnopqrstuvwxyz1234567890?;:ABCDEFGHIJKLMNOPQRSTUVWXYZ^&";
            var num = _random.Next(0, chars.Length - 1);

            return chars[num];
        }
    }
}
