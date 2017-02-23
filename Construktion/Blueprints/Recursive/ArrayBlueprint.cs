namespace Construktion.Blueprints.Recursive
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ArrayBlueprint : Blueprint
    {
        public bool Matches(ConstruktionContext context)
        {
            return context.Request.IsArray;
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var arrayType = context.Request.GetElementType();

            var results = construct(arrayType, pipeline).ToList();

            var array = Array.CreateInstance(arrayType, results.Count);
            
            for (var i = 0; i <= results.Count - 1; i++)
            {
                array.SetValue(results[i], i);
            }

            return array;
        }

        public IEnumerable<object> construct(Type closedType, ConstruktionPipeline pipeline)
        {
            var count = 3;

            for (var i = 0; i < count; i++)
            {
                yield return pipeline.Construct(new ConstruktionContext(closedType));
            }
        }
    }
}