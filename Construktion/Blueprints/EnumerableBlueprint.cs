namespace Construktion.Blueprints
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class EnumerableBlueprint : Blueprint
    {
        public bool Matches(ConstruktionContext context)
        {
            return context.Request.GetTypeInfo().IsGenericType &&
                   context.Request.GetGenericTypeDefinition() == typeof(IEnumerable<>);
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var closedType = context.Request.GenericTypeArguments[0];

            var result = construct(closedType, pipeline);

            var convertedResult = typeof(ConvertedEnumerable<>)
                .MakeGenericType(closedType)
                .GetTypeInfo()
                .DeclaredConstructors
                .Single()
                .Invoke(new[] { result });

            return convertedResult;
        }

        public IEnumerable<object> construct(Type closedType, ConstruktionPipeline pipeline)
        {
            var count = 3;

            for (int i = 0; i < count; i++ )
            {
                yield return pipeline.Construct(new ConstruktionContext(closedType));
            }
        }

        //https://github.com/AutoFixture/AutoFixture/blob/master/Src/AutoFixture/Kernel/EnumerableRelay.cs
        private class ConvertedEnumerable<T> : IEnumerable<T>
        {
            private readonly IEnumerable<object> enumerable;

            public ConvertedEnumerable(IEnumerable<object> enumerable)
            {
                this.enumerable = enumerable;
            }

            public IEnumerator<T> GetEnumerator()
            {
                foreach (var item in this.enumerable)
                    if (item is T)
                        yield return (T)item;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }
}