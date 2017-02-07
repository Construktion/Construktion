namespace Construktion
{
    using System;
    using System.Collections.Generic;
    using Builders;
 
    public class Construktion
    {
        private readonly IEnumerable<Builder> _builders = new List<Builder>
        {
            new AttributeBuilder(),
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
            var pipeline = new DefaultConstruktionPipeline(_builders);

            var result = (T)pipeline.Build(new RequestContext(request));

            afterBuild?.Invoke(result);

            return result;
        }
    }
}
