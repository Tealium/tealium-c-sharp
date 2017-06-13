using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
            
namespace TealiumCSharp
{
	/// <summary>
	/// App data. Adds standard Tealium data variables to library data stream.
	/// </summary>
	public class AppData
	{
		private Dictionary<string, object> _volatileData;

		public AppData()
		{
			_volatileData = new Dictionary<string, object>();
			_volatileData.Add(AppDataConstants.SESSION_ID, NewSessionId());
		}

		/// <summary>
		/// Adds custom data to app data.
		/// </summary>
		/// <param name="data">Data.</param>
		public void AddData(Dictionary<string, object> data)
		{
			_volatileData = Utils.MergeDictionary(_volatileData, data);
		}

		/// <summary>
		/// Retrieves the app data 
		/// </summary>
		/// <returns>App data.</returns>
		public Dictionary<string, object> GetData()
		{
			Dictionary<string, object> data = new Dictionary<string, object>();
			string random = this.GetRandom();
			string timestamp = this.GetTimestampInSeconds();

			data.Add(AppDataConstants.RANDOM, random);
			data.Add(AppDataConstants.TIMESTAMP_EPOCH, timestamp);

			_volatileData = Utils.MergeDictionary(_volatileData, data);
			return _volatileData;
		}

		internal string NewSessionId()
		{
			return GetTimestampInMilliseconds();
		}

		/// <summary>
		/// Resets the session identifier. Use if a new session id is needed prior to next launch.
		/// </summary>
		public void ResetSessionId()
		{
			SetSessionId(NewSessionId());
		}

		/// <summary>
		/// Explicitely sets the session identifier.
		/// </summary>
		/// <param name="sessionId">Session identifier.</param>
		public void SetSessionId(string sessionId)
		{
			Dictionary<string, object> data = new Dictionary<string, object>();
			data.Add(AppDataConstants.SESSION_ID, sessionId);

			AddData(data);

		}

		internal string GetRandom()
		{
			int length = 16;
			Random rd = new Random();

			var result = new StringBuilder();

			for (int i = 0; i < length; i++)
			{
				int num = rd.Next(1, 10);
				result.Append(num.ToString());
			}

			return result.ToString();
		}

		private string GetTimestampInSeconds()
		{
			TimeSpan ts = (DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1));

			return Math.Round(ts.TotalSeconds).ToString();
		}

		private string GetTimestampInMilliseconds()
		{
			TimeSpan ts = (DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1));

			return Math.Round(ts.TotalMilliseconds).ToString();

		}
	}



}
