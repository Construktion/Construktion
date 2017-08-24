namespace Construktion.Debug
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Quick and dirty diagnostic pipeline
    /// </summary>
    internal class DebuggingConstruktionPipeline : ConstruktionPipeline
    {
        private readonly List<string> _log = new List<string>();
        private readonly List<Type> _underConstruction = new List<Type>();
        private int _level = -1;
        public ConstruktionSettings Settings { get; }

        public DebuggingConstruktionPipeline(ConstruktionSettings settings)
        {
            Settings = settings;
        }

        public object Send(ConstruktionContext requestContext)
        {
            return DebugSend(requestContext, out List<string> debugLog);
        }

        public object DebugSend(ConstruktionContext requestContext, out List<string> debugLog)
        {
            var depth = _underConstruction.Count(x => requestContext.RequestType == x);

            var blueprint = Settings.Blueprints.First(x => x.Matches(requestContext));

            if (depth > Settings.RecurssionDepth)
            {
                debugLog = _log;
                return default(object);
            }

            var name = requestContext.RequestType.FullName;
            var start = $"Start: {name}";

            if (requestContext.PropertyInfo != null)
            {
                name = requestContext.PropertyInfo.Name;
                start = $"Start Property: {name}";
            }
            else if (requestContext.ParameterInfo != null)
            {
                name = requestContext.ParameterInfo.Name;
                start = $"Start Parameter: {name}";
            }

            _level++;
            _underConstruction.Add(requestContext.RequestType);

            if (_level != 0)
                _log.Add("");

            var buffer = new string(' ', _level * 5);
            _log.Add($"{buffer}{start}");
            _log.Add($"{buffer}{blueprint}");

            var result = blueprint.Construct(requestContext, this);

            _level--;
            _underConstruction.Remove(requestContext.RequestType);

            if (_log.Last().Trim().StartsWith("End"))
                _log.Add("");

            _log.Add($"{buffer}End {name}");

            debugLog = _log;

            return result;
        }
    }
}