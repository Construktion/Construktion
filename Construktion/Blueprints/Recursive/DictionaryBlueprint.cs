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

            var keys = GenerateUqKeys(howMany, pipeline, new ConstruktionContext(key), new HashSet<object>()).ToList();
            var values = GenerateValues(howMany, value, pipeline).ToList();

            var dictionary = (IDictionary)Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(key, value));

            for (var i = 0; i <= howMany - 1; i++)
            {
               dictionary.Add(keys[i], values[i]);
            }

            return dictionary;
        }

        private HashSet<object> GenerateUqKeys(int howMany, ConstruktionPipeline pipeline, ConstruktionContext context, HashSet<object> items)
        {
            var newItem = pipeline.Construct(context);

            if (newItem != null)
                items.Add(newItem);

            if (items.Count == howMany)
            {
                return items;
            }

            return GenerateUqKeys(howMany, pipeline, context, items);
        }

        private IEnumerable<object> GenerateValues(int howMany, Type closedType, ConstruktionPipeline pipeline)
        {
            for (var i = 0; i < howMany; i++)
            {
                yield return pipeline.Construct(new ConstruktionContext(closedType));
            }
        }
    }
}