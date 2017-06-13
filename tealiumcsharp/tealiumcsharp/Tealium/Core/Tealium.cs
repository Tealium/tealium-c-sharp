using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace TealiumCSharp

{
	/// <summary>
	/// Tealium.
	/// </summary>
	public class Tealium
	{
		private ModulesManager modulesManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:TealiumCSharp.Tealium"/> class.
		/// </summary>
		/// <param name="config">Config.</param>
		public Tealium(Config config)
		{
			modulesManager = new ModulesManager();
			modulesManager.StartModules(config);
		}

		/// <summary>
		/// Track with a specified title.
		/// </summary>
		/// <returns>The track.</returns>
		/// <param name="title">Required title. Does not need to be unique.</param>
		public void Track(string title)
		{
			Track(title, null, null);
		}

		/// <summary>
		/// Track with a specified title and customData.
		/// </summary>
		/// <returns>The track.</returns>
		/// <param name="title">Required title. Does not need to be unique.</param>
		/// <param name="customData">Custom data containing string or string[] values.</param>
		public void Track(string title,
						  Dictionary<string, object> customData)
		{
			Track(title, customData, null);
		}


		/// <summary>
		/// Track with a title, customData and completion.
		/// </summary>
		/// <returns></returns>
		/// <param name="title">Title.</param>
		/// <param name="customData">Custom data.</param>
		/// <param name="completion">Completion.</param>
		public void Track(string title,
						  Dictionary<string, object> customData,
						  TrackCompletion completion)
		{
			Dictionary<string, object> newData = new Dictionary<string, object>();
			newData = Utils.MergeDictionary(newData, customData);
			newData.Add(Constants.EVENT, title);
			modulesManager.Track(newData,
								completion);

		}

		/// <summary>
		/// Enable this instance after a Disable() call.
		/// </summary>
		public void Enable()
		{
			modulesManager.ResumeModules();
		}

		/// <summary>
		/// Disable this instance.
		/// </summary>
		public void Disable()
		{
			modulesManager.PauseModules();
		}

	}
}
