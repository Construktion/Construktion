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

        public object Send(ConstruktionContext context)
        {
            return DebugSend(context, out List<string> debugLog);
        }

        public object DebugSend(ConstruktionContext context, out List<string> debugLog)
        {
            var depth = _underConstruction.Count(x => context.RequestType == x);

            var blueprint = Settings.Blueprints.First(x => x.Matches(context));

            _level++;
            var buffer = new string(' ', _level * 5);

            if (depth > Settings.RecurssionDepth)
            {
                _log.Add($"{buffer}Recursion detected over the allowed limit. Omitting {context.RequestType.FullName}");
                debugLog = _log;

                return default(object);
            }

            var requestName = StartLog(context, blueprint, buffer);

            object result;
            try
            {
                result = blueprint.Construct(context, this);
            }
            catch (Exception ex)
            {
                _log.Add($"{buffer}An exception occurred in blueprint {blueprint.GetType().FullName}, when constructing {requestName}");
                _log.Add($"{buffer}Exception is: {ex.Message}");
                _log.Add("");
                debugLog = _log;

                return default(object);
            }

            EndLog(context, buffer, requestName);

            debugLog = _log;

            return result;
        }

        private string StartLog(ConstruktionContext requestContext, Blueprints.Blueprint blueprint, string buffer)
        {
            var requestName = requestContext.RequestType.FullName;
            var start = $"Start: {requestName}";

            if (requestContext.PropertyInfo != null)
            {
                requestName = requestContext.PropertyInfo.Name;
                start = $"Start Property: {requestName}";
            }
            else if (requestContext.ParameterInfo != null)
            {
                requestName = requestContext.ParameterInfo.Name;
                start = $"Start Parameter: {requestName}";
            }

            _underConstruction.Add(requestContext.RequestType);

            if (_level != 0)
                _log.Add("");

            _log.Add($"{buffer}{start}");
            _log.Add($"{buffer}Blueprint: {blueprint}");

            return requestName;
        }

        private void EndLog(ConstruktionContext context, string buffer, string requestName)
        {
            _level--;
            _underConstruction.Remove(context.RequestType);

            if (_log.Last().Trim().StartsWith("End"))
                _log.Add("");

            _log.Add($"{buffer}End {requestName}");
        }
    }
}