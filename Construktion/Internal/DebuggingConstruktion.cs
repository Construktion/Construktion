// ReSharper disable once CheckNamespace
namespace Construktion.Debug
{
	using System;
	using System.Collections.Generic;
	using Internal;

	public class DebuggingConstruktion
	{
		[Obsolete("This class will be deleted in 2.0. Please use new Construktion().DebuggingConstruct instead.")]
		public DebuggingConstruktion() : this(new Construktion()) { }

		[Obsolete("This class will be deleted in 2.0. Please use new Construktion().DebuggingConstruct instead.")]
		public DebuggingConstruktion(Construktion construktion) { }

		/// <summary>de
		/// DO NOT use for normal operations. Should be used for ad hoc debugging ONLY.
		/// </summary>
		/// <returns></returns>
		[Obsolete("This class will be deleted in 2.0. Please use new Construktion().DebuggingConstruct instead.")]
		public object DebuggingConstruct(ConstruktionContext context, out string log)
		{
			var pipeline = new DebuggingConstruktionPipeline(new DefaultConstruktionSettings());

			var result = pipeline.DebugSend(context, out List<string> debugLog);

			log = string.Join("\n", debugLog);

			return result;
		}
	}
}