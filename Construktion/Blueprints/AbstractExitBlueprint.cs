// ReSharper disable once CheckNamespace
namespace Construktion
{
    using System.Reflection;

    public abstract class AbstractExitBlueprint<T> : ExitBlueprint<T>, ExitBlueprint
    {
        public bool Matches(object item, ConstruktionContext context) =>
            typeof(T).IsAssignableFrom(context.RequestType) && Matches((T)item, context);

        /// <summary>
        /// Match a context to construct. Note: an additional check is
        /// done internally to assure the context is of type T to 
        /// prevent runtime casting errors. 
        /// </summary>
        public virtual bool Matches(T item, ConstruktionContext context) => true;

        public object Construct(object item, ConstruktionPipeline pipeline) => Construct((T)item, pipeline);

        /// <summary>
        /// The final chance to alter an object of T after it 
        /// was already constructed through the pipeline. 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="pipeline"></param>
        /// <returns></returns>
        public abstract T Construct(T item, ConstruktionPipeline pipeline);
    }
}