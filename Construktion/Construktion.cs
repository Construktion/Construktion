namespace Construktion
{
    using System;
    using System.Collections.Generic;
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
                new GuidBuilder()
            };
        }

        public T Build<T>()
        {
            var result = default(T);
            var request = typeof(T);

            foreach (var builder in _builders)
            {
                if (!builder.CanBuild(request))
                    continue;

                result = (T)builder.Build(new RequestContext(request));
                break;
            }

            return result;
        }

        public T Build<T>(Action<T> afterBuild)
        {
            if (afterBuild == null)
                throw new ArgumentNullException(nameof(afterBuild));

            var result = Build<T>();
            afterBuild(result);

            return result;
        }
    }
}
