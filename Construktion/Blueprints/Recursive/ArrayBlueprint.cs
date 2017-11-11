namespace Construktion.Blueprints.Recursive
{
    using System;

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
            var array = Array.CreateInstance(arrayType, pipeline.Settings.EnumuerableCount);

            for (var i = 0; i <= pipeline.Settings.EnumuerableCount - 1; i++)
            {
                var value = pipeline.Send(new ConstruktionContext(arrayType));

                array.SetValue(value, i);
            }

            return array;
        }
    }
}