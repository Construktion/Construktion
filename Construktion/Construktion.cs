namespace Construktion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Builders;

    public class Construktion
    {
        private readonly IEnumerable<Builder> _builders = new List<Builder>
        {
            new StringBuilder(),
            new NumericBuilder(),
            new CharBuilder(),
            new GuidBuilder(),
            new BoolBuilder(),
            new EnumBuilder(),
            new ClassBuilder()
        };

        public T Build<T>()
        {
            return DoBuild<T>(typeof(T), null);
        }

        public T Build<T>(Action<T> afterBuild)
        {
            return DoBuild(typeof(T), afterBuild);
        }

        public object Build(Type type)
        {
            return DoBuild<object>(type, null);
        }

        private T DoBuild<T>(Type request, Action<T> afterBuild)
        {
            var builder = GetBuilder(request);

            var result = (T)builder.Build(new RequestContext(this, request));

            afterBuild?.Invoke(result);

            return result;
        }

        private Builder GetBuilder(Type request)
        {
            var builder = _builders.FirstOrDefault(x => x.CanBuild(request));

            if (builder == null)
                throw new Exception($"No builder can be found for {request.Name}");

            return builder;
        }
    }
}
