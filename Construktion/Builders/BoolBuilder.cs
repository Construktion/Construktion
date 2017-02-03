namespace Construktion.Builders
{
    using System;

    public class BoolBuilder : Builder
    {
        private bool value;

        public bool CanBuild(Type request)
        {
            return request == typeof(bool);
        }

        public object Build(RequestContext context)
        {
            value = !value;

            return value;
        }
    }
}
