using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace TealiumCSharp
{
	/// <summary>
	/// Tealium Config object used to initialize a Tealium library instance.
	/// </summary>
	public class Config
	{
		public string Account, Profile, Environment;
		private string datasource, visitorId, overrideCollectUrl;
		private string[] moduleNames;
		public Dictionary<string, object> OptionalData;

		public string Datasource
		{
			get	{ return datasource; }
			set { datasource = value; }
		}

		public string VisitorId
		{
			get { return visitorId; }
			set { visitorId = value; }
		}

		public string OverrideCollectUrl
		{
			get { return overrideCollectUrl; }
			set { overrideCollectUrl = value; }
		}

		public string[] ModuleNames
		{
			get { return moduleNames; }
			set { moduleNames = value; }
		}

		/// <summary>
		/// Convenience initializer for a new instance of the <see cref="T:TealiumCSharp.Config"/> class.
		/// </summary>
		/// <param name="account">Tealium Account.</param>
		/// <param name="profile">Tealium Profile.</param>
		/// <param name="visitorId">Visitor identifier. Should be 32 alphnumeric characters long.</param>
		public Config(string account, string profile, string visitorId)
			: this(account, profile, null, visitorId, null, null, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:TealiumCSharp.Config"/> class.
		/// </summary>
		/// <param name="account">Tealium Account.</param>
		/// <param name="profile">Tealium Profile.</param>
		/// <param name="environment">Environment.</param>
		/// <param name="visitorId">Visitor identifier. Should be 32 alphanumeric characters long.</param>
		/// <param name="modules">Modules. Modules to initialize with Collect Library</param>
		/// <param name="datasource">Datasource ID provided by Universal Data Hub (UDH) setup</param>
		/// <param name="overrideCollectUrl">Override collect URL. Custom destination URL for data</param>
		/// <param name="optionalData">Optional data dictionary. Use only string or string[] values</param>
		///
		public Config(string account,
					  string profile,
					  string environment,
					  string visitorId,
					  string[] modules,
					  string datasource = null,
					  string overrideCollectUrl = null,
					  Dictionary<string, object> optionalData = null)
		{

			if (account == null)
			{
				throw new ArgumentNullException(nameof(account));
			}
			if (profile == null)
			{
				throw new ArgumentNullException(nameof(profile));
			}
			if (visitorId == null)
			{
				throw new ArgumentNullException(nameof(visitorId));
			}
			if (modules == null)
			{
				modules = Constants.DEFAULT_MODULES;
			}
			//future use
			//if (datasource == null)
			//{
			//	throw new ArgumentNullException(nameof(datasource));
			//}

			this.Account = account;
			this.Profile = profile;
			this.Environment = environment;
			this.Datasource = datasource;
			this.VisitorId = visitorId;
			this.ModuleNames = modules;
			this.OverrideCollectUrl = overrideCollectUrl;

			//add optional data dictionary
			this.OptionalData = optionalData;

		}
	}
}
