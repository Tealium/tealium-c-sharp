using System.Collections.Generic;
using System.Linq;
using TealiumCSharp;

namespace Tests
{
	public struct ModuleResponses
	{
		public bool Enabled;
		public bool Disabled;
		public bool Tracked;
		public bool Reported;
	}

	public class TestsHelper : ModuleDelegate
	{

		ModuleResponses responses = new ModuleResponses();

		// All responses should = true
		public ModuleResponses moduleReturnsFromAllBaseCalls(Module module)
		{

			responses.Enabled = false;
			responses.Disabled = false;
			responses.Tracked = false;
			responses.Reported = false;

			module.ModuleDelegate = this;

			Config testConfig = new Config("test", "test", "test");

			module.Enable(testConfig);

			Dictionary<string, object> testDict = new Dictionary<string, object>();
			Track testTrack = new Track(testDict,
										null,
										null);


			module.Track(testTrack);

			Process testProcess = new Process();

			module.HandleReport(module,
								testProcess);



			module.Disable();

			return responses;

		}

		// All responses should = false
		public ModuleResponses moduleReturnsFromAllBaseCallsFromANonEnabledState(Module module)
		{
			responses.Enabled = false;
			responses.Disabled = false;
			responses.Tracked = false;
			responses.Reported = false;

			module.ModuleDelegate = this;

			Dictionary<string, object> testDict = new Dictionary<string, object>();
			Track testTrack = new Track(testDict,
										null,
										null);


			module.Track(testTrack);

			Process testProcess = new Process();

			module.HandleReport(module,
								testProcess);



			return responses;

		}

		// Tracked and Reported should = false
		public ModuleResponses moduleReturnsFromAllBaseCallsAfterDisable(Module module)
		{
			responses.Enabled = false;
			responses.Disabled = false;
			responses.Tracked = false;
			responses.Reported = false;

			module.ModuleDelegate = this;


			Config testConfig = new Config("test", "test", "test");

			module.Enable(testConfig);

			module.Enable(testConfig);
			module.Disable();

			Dictionary<string, object> testDict = new Dictionary<string, object>();
			Track testTrack = new Track(testDict,
										null,
										null);


			module.Track(testTrack);

			Process testProcess = new Process();

			module.HandleReport(module,
								testProcess);



			return responses;

		}

		// MODULE DELEGATES

		public void ModuleFinished(Module module,
						   Process process)
		{
			if (process.type == ProcessType.Enable)
			{
				responses.Enabled = true;
			}
			if (process.type == ProcessType.Disable)
			{
				responses.Disabled = true;
			}
			if (process.type == ProcessType.Track)
			{
				responses.Tracked = true;
			}

		}

		public void ModuleFinishedReport(Module fromModule,
										 Module module,
										 Process process)
		{
			responses.Reported = true;
		}

		public void ModuleRequests(Module module,
								   Process process)
		{

		}

		public static bool AreDictionariesEqual(Dictionary<string, object> dict,
												Dictionary<string, object> dict2)
		{

			if (dict.Count != dict2.Count)
			{
				System.Diagnostics.Debug.WriteLine("TestsHelper: AreDictionariesEqual: dict count mismatch.");
				return false;
			}

			//foreach (var pair in dict)
			foreach (KeyValuePair<string, object> kvp in dict)
			{

				object dictValue = kvp.Value;
				object dict2Value = dict2[kvp.Key];

				if ((dict2.ContainsKey(kvp.Key)) == false)
				{
					return false;
				}

				if (dictValue.GetType() != dict2Value.GetType())
				{
					return false;
				}

				if (dictValue is System.String)
				{
					if (dictValue != dict2Value)
					{
						return false;
					}
				}

				if (dictValue is System.String[])
				{
					return Enumerable.SequenceEqual(dictValue as string[], dict2Value as string[]);
				}

			}

			return true;
		}

		public static bool DictionaryContainsSubDictionary(Dictionary<string, object> dictionary,
														   Dictionary<string, object> subDictionary)
		{
			foreach (KeyValuePair<string, object> kvp in subDictionary)
			{
				object dictValue = kvp.Value;

				if (!dictionary.ContainsKey(kvp.Key))
				{
					return false;
				}

				object dict2Value = subDictionary[kvp.Key];

				if (dictValue.GetType() != dict2Value.GetType())
				{
					return false;
				}

				if (dictValue is System.String)
				{
					if (dictValue != dict2Value)
					{
						return false;
					}
				}

				if (dictValue is System.String[])
				{
					return Enumerable.SequenceEqual(dictValue as string[], dict2Value as string[]);
				}

			}
			return true;
		}

	}

}
