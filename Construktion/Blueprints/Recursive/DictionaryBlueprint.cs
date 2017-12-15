//This is disabled due to a bug in resharper
// ReSharper disable RedundantAssignment
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
            var itemCount = 4;

            var key = context.RequestType.GetGenericArguments()[0];
            var valueType = context.RequestType.GetGenericArguments()[1];

            if (key.GetTypeInfo().IsEnum)
                itemCount = Enum.GetNames(key).Length;

            var keys = createUniqueKeys(new HashSet<object>()).ToList();
            var values = createValues(valueType).ToList();

            var dictionary = (IDictionary)typeof(Dictionary<,>).NewGeneric(key, valueType);

            for (var i = 0; i <= itemCount - 1; i++)
                dictionary.Add(keys[i], values[i]);

            return dictionary;

            HashSet<object> createUniqueKeys(HashSet<object> items)
            {
                var newItem = pipeline.Send(new ConstruktionContext(key));

                if (newItem != null)
                    items.Add(newItem);

                return items.Count == itemCount
                    ? items
                    : createUniqueKeys(items);
            }

            IEnumerable<object> createValues(Type closedType)
            {
                for (var i = 0; i < itemCount; i++)
                    yield return pipeline.Send(new ConstruktionContext(closedType));
            }
        }
    }
}