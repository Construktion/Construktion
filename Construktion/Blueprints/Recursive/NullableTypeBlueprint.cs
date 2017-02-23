namespace Construktion.Blueprints.Recursive
{
    using System;
    using System.Reflection;

    public class NullableTypeBlueprint : Blueprint
    {
        private readonly Random _rand = new Random();

        public bool Matches(ConstruktionContext context)
        {
            return context.RequestType.GetTypeInfo().IsGenericType &&
                   context.RequestType.GetTypeInfo().GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var closedType = context.RequestType.GetGenericArguments()[0];

            var useNull = _rand.Next(1, 5);

            return useNull == 1 
                ? null 
                : pipeline.Construct(new ConstruktionContext(closedType));
        }
    }
}