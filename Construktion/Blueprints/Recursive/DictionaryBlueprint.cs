namespace Construktion.Blueprints.Recursive
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Internal;

    public class DictionaryBlueprint : Blueprint
    {
        public bool Matches(ConstruktionContext context)
        {
            var typeInfo = context.RequestType.GetTypeInfo();

            return typeInfo.IsGenericType &&
                   typeInfo.GetGenericTypeDefinition() == typeof(Dictionary<,>);
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var count = 4;

            var key = context.RequestType.GetGenericArguments()[0];
            var value = context.RequestType.GetGenericArguments()[1];

            if (key.GetTypeInfo().IsEnum)
                count = Enum.GetNames(key).Length;

            var keys = CreateUniqueKeys(count, key, pipeline, new HashSet<object>()).ToList();
            var values = Values(count, value, pipeline).ToList();

            var dictionary = (IDictionary)typeof(Dictionary<,>).NewGeneric(key, value);

            for (var i = 0; i <= count - 1; i++)
                dictionary.Add(keys[i], values[i]);

            return dictionary;
        }

        private HashSet<object> CreateUniqueKeys(int count, Type key, ConstruktionPipeline pipeline,
            HashSet<object> items)
        {
            var newItem = pipeline.Send(new ConstruktionContext(key));

            if (newItem != null)
                items.Add(newItem);

            return items.Count == count
                       ? items
                       : CreateUniqueKeys(count, key, pipeline, items);
        }

        private IEnumerable<object> Values(int count, Type closedType, ConstruktionPipeline pipeline)
        {
            for (var i = 0; i < count; i++)
                yield return pipeline.Send(new ConstruktionContext(closedType));
        }
    }
}