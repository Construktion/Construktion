// ReSharper disable once CheckNamespace
namespace Construktion
{
    using System;

    /// <summary>
    /// A blueprint that will construct the specified type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractBlueprint<T> : Blueprint
    {
        internal readonly Random _random = new Random();

        /// <inheritdoc />
        bool Blueprint.Matches(ConstruktionContext context) => context.RequestType == typeof(T) && Matches(context);

        /// <summary>
        /// Matches types of the closed generic. Can be overridden in derived classes.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual bool Matches(ConstruktionContext context) => true;

        /// <inheritdoc />
        object Blueprint.Construct(ConstruktionContext context, ConstruktionPipeline pipeline) => Construct(context, pipeline);

        /// <summary>
        /// Construct an object of T.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="pipeline"></param>
        /// <returns></returns>
        public abstract T Construct(ConstruktionContext context, ConstruktionPipeline pipeline);
    }
}