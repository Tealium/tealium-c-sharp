using System;

namespace TealiumCSharp
{

	public interface ModuleDelegate
	{
		void ModuleFinished(Module module,
							Process process);

		void ModuleFinishedReport(Module fromModule,
								Module module,
								Process process);

		void ModuleRequests(Module module,
							Process process);
	}

	/// <summary>
	/// Base Tealium Module class. Meant to be subclassed by modules.
	/// </summary>
	public class Module : IComparable<Module>
	{
		public ModuleDelegate ModuleDelegate = null;
		public string NameId = "Tealium Module: Base Class";
		public bool IsEnabled = false;
		public int Build = 1;


		public override int GetHashCode()
		{
			return NameId.GetHashCode();
		}

		public Module()
		{
		}

		public virtual void Enable(Config config)
		{
			this.IsEnabled = true;
			DidFinishEnable();

		}

		public virtual void Track(Track track)
		{
			DidFinishTrack(track);
		}

		public virtual void HandleReport(Module fromModule,
										 Process process)
		{
			DidFinishReport(fromModule, process);

		}

		public virtual void Disable()
		{
			this.IsEnabled = false;
			DidFinishDisable();

		}


		public virtual void Auto(Process process, Config config)
		{
			switch (process.type)
			{
				case ProcessType.Enable:
					Enable(config);
					break;
				case ProcessType.Disable:
					Disable();
					break;
				case ProcessType.Track:
					//TODO: maybe some error checking here before assinging the track 
					Track(process.track);
					break;

			}
		}

		public void DidFinishEnable()
		{

			var process = new Process(ProcessType.Enable,
														true,
														null,
														null);

			ModuleDelegate?.ModuleFinished(this, process);

		}

		public void DidFailToEnable(Exception error)
		{

			var process = new Process(ProcessType.Enable,
													   false,
													   null,
													   error);

			ModuleDelegate?.ModuleFinished(this,
										  process);

		}

		public void DidFinishDisable()
		{

			var process = new Process(ProcessType.Disable,
														true,
														null,
														null);

			ModuleDelegate?.ModuleFinished(this,
										   process);

		}

		public void DidFailToDisable(Exception error)
		{

			var process = new Process(ProcessType.Disable,
														false,
														null,
														error);

			ModuleDelegate?.ModuleFinished(this,
										  process);

		}

		public void DidFinishTrack(Track track)
		{

			var process = new Process(ProcessType.Track,
														true,
														track,
														null);


			ModuleDelegate?.ModuleFinished(this,
										  process);

		}

		public void DidFailToTrack(Track track, Exception error)
		{

			var process = new Process(ProcessType.Track,
														false,
														track,
														error);

			ModuleDelegate?.ModuleFinished(this,
										  process);

		}

		public void DidFinishReport(Module fromModule, Process process)
		{

			ModuleDelegate?.ModuleFinishedReport(fromModule,
												  this,
												  process);

		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;

			Module objAsModule = obj as Module;
			if (objAsModule == null)
			{
				return false;
			}
			else
			{
				return Equals((Module)objAsModule);
			}
		}

		public int CompareTo(Module module)
		{
			return String.CompareOrdinal(NameId, module.NameId);
		}
	}
}
