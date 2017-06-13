using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace TealiumCSharp
{
	public class CollectModule : Module
	{
		Collect Collect;
		public const string Name = "TealiumCSharp.CollectModule";

		public CollectModule()
		{
			this.NameId = Name;
			this.Build = 1;
		}

		public override void Enable(Config config)
		{
			string baseUrl = Collect.DefaultBaseURLString();
			if (config.OverrideCollectUrl != null)
			{
				baseUrl = config.OverrideCollectUrl;
			}
			Collect = new Collect(config, baseUrl);
			IsEnabled = true;
			DidFinishEnable();
		}

		public override void Disable()
		{
			IsEnabled = false;
			Collect = null;
			DidFinishDisable();
		}

		public override void Track(Track track)
		{
			// Bail out if module disabled or Collect was not started.
			if (IsEnabled == false)
			{
				DidFinishTrack(track);
				return;
			}
			if (Collect == null)
			{
				DidFinishTrack(track);
				return;
			}

			Collect.Dispatch(track.data, (exception) =>
			{
				if (exception != null)
				{
					// Notify any completion handler
					safelyTriggerCompletionFor(track,
													false,
													exception);
					DidFailToTrack(track, exception);
				}
				else
				{
					// Notify any completion handler
					safelyTriggerCompletionFor(track,
													true,
													null);
					DidFinishTrack(track);
				}
			}
			);
		}

		public void safelyTriggerCompletionFor(Track track,
											   bool success,
											   Exception callBack)
		{
			if (track.completion == null)
			{
				return;
			}
			track.completion(success,
							 track.info,
							 callBack);
		}

		public override void HandleReport(Module fromModule, Process process)
		{
			base.HandleReport(fromModule, process);
		}
	}
}
