using System.Collections.Generic;

namespace Construktion.Debug
{
    public class DebuggingConstruktion
    {
        private readonly Construktion _construktion;

        public DebuggingConstruktion() : this (new Construktion())
        {
            
        }

        public DebuggingConstruktion(Construktion construktion)
        {
            _construktion = construktion;
        }

        /// <summary>
        /// DO NOT use for normal operations. Should be used for ad hoc debugging ONLY.
        /// </summary>
        /// <returns></returns>
        public object DebuggingConstruct(ConstruktionContext context, out string log)
        {
            var pipeline = new DebuggingConstruktionPipeline(_construktion.Registry.ToSettings());

            var result = pipeline.DebugSend(context, out List<string> debugLog);

            log = string.Join("\n", debugLog);

            return result;
        }
    }
}
