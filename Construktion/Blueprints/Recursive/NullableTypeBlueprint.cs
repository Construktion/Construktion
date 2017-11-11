namespace Construktion.Blueprints.Recursive
{
    using System;
    using System.Reflection;

    public class NullableTypeBlueprint : Blueprint
    {
        private readonly Random _random = new Random();

        public bool Matches(ConstruktionContext context)
        {
            var typeInfo = context.RequestType.GetTypeInfo();

            return typeInfo.IsGenericType &&
                   typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var closedType = context.RequestType.GetGenericArguments()[0];

            var useNull = _random.Next(1, 5);

            return useNull == 1
                       ? null
                       : pipeline.Send(new ConstruktionContext(closedType));
        }
    }
}