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
            var items = pipeline.Settings.EnumerableCount;

            var key = context.RequestType.GetGenericArguments()[0];
            var valueType = context.RequestType.GetGenericArguments()[1];

            if (key.GetTypeInfo().IsEnum)
                items = Enum.GetNames(key).Length;

            var keys = uniqueKeys();
            var values = itemValues(valueType);

            var dictionary = (IDictionary)typeof(Dictionary<,>).NewGeneric(key, valueType);

            for (var i = 0; i <= items - 1; i++)
                dictionary.Add(keys[i], values[i]);

            return dictionary;

            List<object> uniqueKeys()
            {
                var builtKeys = new HashSet<object>();
                while (true)
                {
                    var newItem = pipeline.Send(new ConstruktionContext(key));

                    if (newItem != null)
                        builtKeys.Add(newItem);

                    if (builtKeys.Count == items)
                        return builtKeys.ToList();
                }
            }

            List<object> itemValues(Type closedType)
            {
                var results = new List<object>();

                for (var i = 0; i < items; i++)
                    results.Add(pipeline.Send(new ConstruktionContext(closedType)));

                return results;
            }
        }
    }
}