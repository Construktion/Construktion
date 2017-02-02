namespace Construktion.Builders
{
    using System;

    public class StringBuilder : Builder
    {
        private readonly Random _random = new Random();

        public bool CanBuild(Type request)
        {
            return request == typeof(string);
        }

        public object Build(RequestContext context)
        {
            var result = context.Request.Name + "-" + _random.Next(1, 9999);

            return result;
        }
    }
}