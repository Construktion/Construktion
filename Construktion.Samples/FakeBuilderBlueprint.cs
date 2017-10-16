using System;
using Construktion.Blueprints;
using FakeItEasy;

namespace Construktion.Samples
{
    public class FakeBuilderBlueprint : Blueprint
    {
        public bool Matches(ConstruktionContext context)
        {
            return context.ParameterInfo?.Name.StartsWith("fake") ?? false;
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var method = typeof(A)
                .GetMethod("Fake", Type.EmptyTypes)
                .MakeGenericMethod(context.ParameterInfo.ParameterType);

            var fake = method.Invoke(null, null);

            return fake;
        }
    }
}