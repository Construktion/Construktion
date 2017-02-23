namespace Construktion.Blueprints.Recursive
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
                   context.Request.GetInterfaces().Contains(typeof(IEnumerable));
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var closedType = context.Request.GenericTypeArguments[0];

            var items = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(closedType));

            var results = construct(closedType, pipeline).ToList();

            results.ForEach(x => items.Add(x));

            return items;
        }

        public IEnumerable<object> construct(Type closedType, ConstruktionPipeline pipeline)
        {
            var count = 3;

            for (var i = 0; i < count; i++ )
            {
                yield return pipeline.Construct(new ConstruktionContext(closedType));
            }
        }
    }
}