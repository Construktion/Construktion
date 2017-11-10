using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Construktion.Blueprints.Recursive
{
    public class EnumerableBlueprint : Blueprint
    {
        public bool Matches(ConstruktionContext context)
        {
            return context.RequestType.GetTypeInfo().IsGenericType &&
                   typeof(IEnumerable).IsAssignableFrom(context.RequestType);
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var closedType = context.RequestType.GenericTypeArguments[0];

            var results = construct(closedType, pipeline);

            return results;
        }

        private IList construct(Type closedType, ConstruktionPipeline pipeline)
        {
            var items = (IList)ReflectionCache.NewGeneric(typeof(List<>), closedType);

            for (var i = 0; i < pipeline.Settings.EnumuerableCount; i++)
            {
                var result = pipeline.Send(new ConstruktionContext(closedType));

                items.Add(result);
            }

            return items;
        }
    }
}