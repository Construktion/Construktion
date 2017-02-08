namespace Construktion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Builders;
 
    public class Construktion
    {
        private readonly List<Builder> _builders = new List<Builder>
        {
            new StringBuilder(),
            new NumericBuilder(),
            new CharBuilder(),
            new GuidBuilder(),
            new BoolBuilder(),
            new EnumBuilder(),
            new ClassBuilder()
        };

        public IList<Builder> Builders => _builders;

        public Construktion()
        {
        }

        public Construktion(Builder additionalBuilder) : this (new List<Builder> { additionalBuilder })
        {
        }

        public Construktion(IList<Builder> additionalBuilders)
        {
            if (additionalBuilders.Any(x => x == null))
                throw new ArgumentNullException(nameof(additionalBuilders), "There are items in the list that are null");

            _builders.InsertRange(0, additionalBuilders);
        }

        public T Build<T>()
        {
            return DoBuild<T>(typeof(T), null);
        }

        public T Build<T>(Action<T> hardCodes)
        {
            return DoBuild(typeof(T), hardCodes);
        }

        public object Build(Type type)
        {
            return DoBuild<object>(type, null);
        }

        private T DoBuild<T>(Type request, Action<T> hardCodes)
        {
            var pipeline = new DefaultConstruktionPipeline(_builders);

            var result = (T)pipeline.Build(new RequestContext(request));

            hardCodes?.Invoke(result);

            return result;
        }
    }
}
