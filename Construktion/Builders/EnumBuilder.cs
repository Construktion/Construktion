namespace Construktion.Builders
{
    using System;
    using System.Reflection;

    public class EnumBuilder : Builder 
    {
        private readonly Random _random = new Random();

        public bool CanBuild(Type request)
        {
            return request.GetTypeInfo().IsEnum;
        }

        public object Build(RequestContext context)
        {
            var values = Enum.GetValues(context.Request);

            return values.GetValue(_random.Next(values.Length));
        }
    }
}
