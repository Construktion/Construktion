namespace Construktion.Blueprints.Recursive
{
    using System;

    public class ArrayBlueprint : Blueprint
    {
        private readonly int _count;

        public ArrayBlueprint() : this(3)
        {
            
        }

        public ArrayBlueprint(int count)
        {
            _count = count;
        }
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
            var array = Array.CreateInstance(arrayType, _count);

            for (var i = 0; i <= _count - 1; i++)
            {
                var value = pipeline.Send(new ConstruktionContext(arrayType));

                array.SetValue(value, i);
            }

            return array;
        }
    }
}