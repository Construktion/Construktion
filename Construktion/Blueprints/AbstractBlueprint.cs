namespace Construktion.Blueprints
{
    using System;

    /// <summary>
    /// A blueprint that will construct the specified type
    /// </summary>
    /// <typeparam name="T"></typeparam>
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