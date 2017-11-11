namespace Construktion.Debug
{
    using System;
    using System.Collections.Generic;

    public class DebuggingConstruktion
    {
        [Obsolete("Please use new Construktion().DebuggingConstruct instead.")]
        public DebuggingConstruktion() : this(new Construktion()) { }

        [Obsolete("Please use new Construktion().DebuggingConstruct instead.")]
        public DebuggingConstruktion(Construktion construktion) { }

        /// <summary>
        /// DO NOT use for normal operations. Should be used for ad hoc debugging ONLY.
        /// </summary>
        /// <returns></returns>
        [Obsolete("Please use new Construktion().DebuggingConstruct instead.")]
        public object DebuggingConstruct(ConstruktionContext context, out string log)
        {
            //todo
            var pipeline = new DebuggingConstruktionPipeline(new DefaultConstruktionSettings());

            var result = pipeline.DebugSend(context, out List<string> debugLog);

            log = string.Join("\n", debugLog);

            return result;
        }
    }
}