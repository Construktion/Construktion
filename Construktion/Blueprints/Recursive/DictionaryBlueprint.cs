namespace Construktion.Blueprints.Recursive
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class DictionaryBlueprint : Blueprint
    {
        public bool Matches(ConstruktionContext context)
        {
            return context.Request.GetTypeInfo().IsGenericType &&
                   context.Request.GetTypeInfo().GetGenericTypeDefinition() == typeof(Dictionary<,>);
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var howMany = 4;

            var key = context.Request.GetGenericArguments()[0];
            var value = context.Request.GetGenericArguments()[1];

            var keys = UniqueKeys(howMany, key, pipeline, new HashSet<object>()).ToList();

            var values = Values(howMany, value, pipeline).ToList();

            var dictionary = (IDictionary)Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(key, value));

            for (var i = 0; i <= howMany - 1; i++)
            {
               dictionary.Add(keys[i], values[i]);
            }

            return dictionary;
        }

        private HashSet<object> UniqueKeys(int howMany, Type key, ConstruktionPipeline pipeline, HashSet<object> items)
        {
            var newItem = pipeline.Construct(new ConstruktionContext(key));

            if (newItem != null)
                items.Add(newItem);

            if (items.Count == howMany)
            {
                return items;
            }

            return UniqueKeys(howMany, key, pipeline, items);
        }

        private IEnumerable<object> Values(int howMany, Type closedType, ConstruktionPipeline pipeline)
        {
            for (var i = 0; i < howMany; i++)
            {
                yield return pipeline.Construct(new ConstruktionContext(closedType));
            }
        }
    }
}