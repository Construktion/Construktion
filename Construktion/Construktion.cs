namespace Construktion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Builders;

    public class Construktion
    {
        private readonly IEnumerable<Builder> _builders;

        public Construktion()
        {
            _builders = new List<Builder>
            {
                new StringBuilder(),
                new NumericBuilder(),
                new CharBuilder(),
                new GuidBuilder(),
                new BoolBuilder(),
                new EnumBuilder()
            };
        }

        public T Build<T>()
        {
            var request = typeof(T);

            var builder = _builders.FirstOrDefault(x => x.CanBuild(request));

            var result = (T)builder.Build(new RequestContext(request));

            return result;
        }

        public T Build<T>(Action<T> afterBuild) where T : class
        {
            if (afterBuild == null)
                throw new ArgumentNullException(nameof(afterBuild));

            var result = Build<T>();
            afterBuild(result);

            return result;
        }
    }
}
