namespace Construktion.Builders
{
    using System;

    public class CharBuilder : Builder
    {
        private readonly Random _random = new Random();

        public bool CanBuild(Type request)
        {
            return request == typeof(char);
        }

        public object Build(RequestContext context)
        {
            var chars = "$%#@!*abcdefghijklmnopqrstuvwxyz1234567890?;:ABCDEFGHIJKLMNOPQRSTUVWXYZ^&";
            var num = _random.Next(0, chars.Length - 1);

            return chars[num];
        }
    }
}
