namespace Construktion.Blueprints
{
    using System;

    public abstract class AbstractBlueprint<T> : Blueprint
    {
        internal readonly Random _random = new Random();

        public virtual bool Matches(ConstruktionContext context)
        {
            return context.RequestType == typeof(T);
        }

        public abstract object Construct(ConstruktionContext context, ConstruktionPipeline pipeline);
    }
}