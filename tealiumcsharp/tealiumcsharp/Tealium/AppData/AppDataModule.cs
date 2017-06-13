using System;
using System.Collections.Generic;
namespace TealiumCSharp
{
	public class AppDataModule : Module
	{
		AppData AppData;
		public const string Name = "TealiumCSharp.AppDataModule";

		public AppDataModule()
		{
			this.NameId = Name;
			this.Build = 1;
		}

		public override void Enable(Config config)
		{
			AppData = new AppData();

			Dictionary<string, object> currentData = new Dictionary<string, object>()
				{
					{Constants.ACCOUNT, config.Account},
					{Constants.PROFILE, config.Profile},
					{Constants.VISITOR_ID, config.VisitorId},
					{Constants.LIBRARY_NAME, Constants.LIBRARY_NAME_VALUE},
					{Constants.LIBRARY_VERSION, Constants.LIBRARY_VERSION_VALUE}
				};

			if (config.Environment != null)
			{
				currentData.Add(Constants.ENVIRONMENT, config.Environment);
			}

			if (config.Datasource != null)
			{
				currentData.Add(Constants.DATASOURCE, config.Datasource);
			}

			AppData.AddData(currentData);


			IsEnabled = true;
			DidFinishEnable();
		}

		public override void Disable()
		{

			IsEnabled = false;
			AppData = null;
			DidFinishDisable();

		}

		public override void Track(Track track)
		{
			if (IsEnabled == false)
			{
				//Todo...
			}
			if (AppData == null)
			{
				// Simply pass to next module - Unfortunately this may give illusion that the module processed normally. 
				this.DidFinishTrack(track);
				return;
			}

			Dictionary<string, object> newData = new Dictionary<string, object>();
			Dictionary<string, object> appDataData = AppData.GetData();
			newData = Utils.MergeDictionary(newData, appDataData);
			newData = Utils.MergeDictionary(newData, track.data);

			Track newTrack = new Track(newData, track.info, track.completion);
			base.DidFinishTrack(newTrack);
		}

	}
}
