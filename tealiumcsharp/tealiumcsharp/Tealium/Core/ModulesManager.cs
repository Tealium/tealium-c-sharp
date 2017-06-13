using System;
using System.Diagnostics;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace TealiumCSharp
{
	public class ModulesManager : ModuleDelegate
	{
		Config Config;
		List<Module> Modules;
		bool IsEnabled;

		public ModulesManager()
		{
		}

		public void Enable(Config config)
		{
			IsEnabled = true;
			Process enableProcess = new Process(ProcessType.Enable, false, null, null);
			Module first = Modules.First();
			first.Auto(enableProcess, config);
		}

		public void StartModules(Config config)
		{
			if (Modules != null)
			{
				return;
			}
			Config = config;
			//loop through modules and create instance
			Modules = new List<Module>();
			for (int index = 0; index < Config.ModuleNames.Length; index++)
			{
				Module module = CreateModule(Config.ModuleNames[index]);
				if (module == null)
				{
					// Module does not exist
					continue;
				}

				module.ModuleDelegate = this;
				Modules.Add(module);
			}

			Enable(config);

		}

		public void PauseModules()
		{
			IsEnabled = false;
		}

		public void ResumeModules()
		{
			IsEnabled = true;
		}

		public void Track(Dictionary<string, object> map,
						  TrackCompletion completion)
		{
			if (IsEnabled == false)
			{
				Debug.WriteLine("Tealium disabled. Ignoring track call.");
				return;
			}

			Track track = new Track(map, null, completion);
			Module module = Modules.First();
			module.Track(track);
		}

		public static Module CreateModule(string strFullyQualifiedName)
		{
			Type type = Type.GetType(strFullyQualifiedName);
			if (type == null)
				return null;

			var module = Activator.CreateInstance(type) as Module;

			return module;
		}

		public Module GetModule(string module)
		{
			foreach (Module mod in Modules)
			{
				if (mod.NameId.Equals(module))
				{
					return mod;
				}
			}
			return null;
		}

		// MODULE DELEGATE

		public void ModuleFinished(Module module,
								   Process process)
		{
			Modules.First()?.HandleReport(module, process);
			Module nextModule = ModuleAfter(module);
			nextModule?.Auto(process, Config);
		}

		public void ModuleFinishedReport(Module fromModule,
										 Module module,
										 Process process)
		{
			Module nextModule = ModuleAfter(module);
			nextModule?.HandleReport(fromModule, process);
		}

		public void ModuleRequests(Module module,
								   Process process)
		{

			if (process.type == ProcessType.Track)
			{
				Track(process.track.data,
						  null);
			}
		}

		// HELPERS

		public Module ModuleAfter(Module module)
		{
			if (Modules.Last() == module)
			{
				return null;
			}
			Module next = null;
			for (int i = 0; i < Modules.Count; i++)
			{
				if (Modules[i] == module)
				{
					next = Modules[i + 1];
					break;
				}
			}
			return next;
		}

	}

}
