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
            return context.RequestType.GetTypeInfo().IsGenericType &&
                   context.RequestType.GetInterfaces().Contains(typeof(IEnumerable));
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var closedType = context.RequestType.GenericTypeArguments[0];

            var results = construct(closedType, pipeline);

            return results;
        }

        private IList construct(Type closedType, ConstruktionPipeline pipeline)
        {
            var count = 3;
            var items = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(closedType));

            for (var i = 0; i < count; i++)
            {
                var result = pipeline.Construct(new ConstruktionContext(closedType));

                items.Add(result);
            }

            return items;
        }
    }
}