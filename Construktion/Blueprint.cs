namespace Construktion
{
    public interface Blueprint
    {
        /// <summary>
        /// When true the blueprint will construct the object
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        bool Matches(ConstruktionContext context);

        /// <summary>
        /// Construct an object
        /// </summary>
        /// <param name="context"></param>
        /// <param name="pipeline"></param>
        /// <returns></returns>
        object Construct(ConstruktionContext context, ConstruktionPipeline pipeline);
    }
}