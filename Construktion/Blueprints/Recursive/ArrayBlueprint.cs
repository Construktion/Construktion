namespace Construktion.Blueprints.Recursive
{
    using System;
    using System.Collections.Generic;

    public class ArrayBlueprint : Blueprint
    {
        public bool Matches(ConstruktionContext context)
        {
            return context.RequestType.IsArray;
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var arrayType = context.RequestType.GetElementType();

            var results = construct(arrayType, pipeline);

            return results;
        }

        private Array construct(Type arrayType, ConstruktionPipeline pipeline)
        {
            var count = 3;

            var items = new List<object>();
            for (var i = 0; i < count; i++)
            {
                items.Add(pipeline.Construct(new ConstruktionContext(arrayType)));
            }

            var array = Array.CreateInstance(arrayType, items.Count);

            for (var i = 0; i <= items.Count - 1; i++)
            {
                array.SetValue(items[i], i);
            }

            return array;
        }
    }
}