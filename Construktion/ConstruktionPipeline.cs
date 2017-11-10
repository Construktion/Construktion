using System;

namespace Construktion
{
    public interface ConstruktionPipeline
    {
        /// <summary>
        /// The configured settings.
        /// </summary>
        ConstruktionSettings Settings { get; }

        /// <summary>
        /// Send a request through the pipeline to be constructed.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        object Send(ConstruktionContext context);

        /// <summary>
        /// Inject an object that will be used whenever a value of that type is requested. Injected objects are scoped to a Construktion instance.
        /// </summary>
        /// <param name="requestType"></param>
        /// <param name="value"></param>
        void Inject(Type requestType, object value);
    }
}
