namespace Construktion.Debug
{
    using System.Collections.Generic;
    using global::Construktion;
    using Construktion.Debug;

    public class DebuggingConstruktion
    {
        private readonly global::Construktion.Construktion _construktion;

        public DebuggingConstruktion() : this (new global::Construktion.Construktion())
        {
            
        }

        public DebuggingConstruktion(global::Construktion.Construktion construktion)
        {
            _construktion = construktion;
        }

        /// <summary>
        /// DO NOT use for normal operations. Should be used for ad hoc debugging ONLY.
        /// </summary>
        /// <returns></returns>
        public object DebuggingConstruct(ConstruktionContext context, out string debugLog)
        {
            var pipeline = new DebuggingConstruktionPipeline(_construktion._registry.GetSettings());

            var result = pipeline.DebugSend(context, out List<string> log);

            debugLog = string.Join("\n", log);

            return result;
        }
    }
}
