namespace Construktion.Blueprints.Recursive
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using Internal;

    public class EnumerableBlueprint : Blueprint
    {
        public bool Matches(ConstruktionContext context) => context.RequestType.GetTypeInfo().IsGenericType &&
                                                            typeof(IEnumerable).IsAssignableFrom(context.RequestType);

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var closedType = context.RequestType.GenericTypeArguments[0];

            var items = (IList)typeof(List<>).NewGeneric(closedType);

            for (var i = 0; i < pipeline.Settings.EnumuerableCount; i++)
            {
                var result = pipeline.Send(new ConstruktionContext(closedType));

                items.Add(result);
            }

            return items;
        }
    }
}