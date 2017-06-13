using System;
using System.Diagnostics;
using TealiumCSharp;
using System.Collections.Generic;

namespace Sample
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Config config = new Config("tealiummobile",
									   "demo",
									   "dev",
									   "612040730c8c11e6b4cbacbc329f41b7",
									   new string[] {
											AppDataModule.Name,
											CollectModule.Name,
											LoggerModule.Name
										},
									   null,
									   null);

			//config.OverrideCollectUrl = "https://tealium.com/";

			Tealium tealium = new Tealium(config);

			Dictionary<string, object> data = new Dictionary<string, object>()
			{
				{"tealium_random", "1249850201034949"},
				{"foo1", new string[]{"12", "23", "123", "1234"}},
				{"foo2", "bar2"},
				{"foo3", "bar3"}

			};

			tealium.Track("someTitle", data);

			//tealium.Track("someTitle",
			//			  data,
			//			  (success, info, error) =>
			//			  {
			//				Debug.WriteLine("Track action call back triggered.");
			//				Debug.WriteLine("Success: " + success + " info: " + info + " error: " + error);
			//			  });
			//}
		}
	}
}
