using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace TealiumCSharp
{
    public class Collect
    {
        Config Config;
        string BaseURL;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TealiumCSharp.Collect"/> class.
        /// </summary>
        /// <param name="config">Config.</param>
        /// <param name="baseURL">Base URL.</param>
        public Collect(Config config, string baseURL)
        {
            Config = config;
            BaseURL = baseURL;
            Debug.WriteLine("Tealium Collect: initialized for url: {0}", BaseURL);
        }

        /// <summary>
        /// Defaults the base URL string.
        /// </summary>
        /// <returns>The base URL string.</returns>
        public static string DefaultBaseURLString()
        {
            return "https://collect.tealiumiq.com/event";
        }

        /// <summary>
        /// Dispatch the specified payload and callback.
        /// </summary>
        /// <param name="payload">Payload.</param>
        /// <param name="callback">Callback.</param>
        public void Dispatch(Dictionary<string, object> payload,
                             Action<Exception> callback)
        {
            Dictionary<string, object> sanitizedCopy = Utils.Sanitized(payload);
            Send(sanitizedCopy, callback);
        }

        internal void Send(Dictionary<string, object> parameters, Action<Exception> callback)
        {
            Debug.WriteLine("Collect.cs -- SEND METHOD");

            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response;

                client.BaseAddress = Config.OverrideCollectUrl != null ? new Uri(Config.OverrideCollectUrl) : new Uri(BaseURL);

                if (Config.Method == Method.POST)
                {
                    //i.gif must be added outside base url - per c# implementation
                    HttpContent httpContent = new StringContent(Utils.DictionaryToJsonString(parameters), Encoding.UTF8, "application/json");
                    response = client.PostAsync("", httpContent).Result;
                }
                else
                {
                    string parameterString = EncodedParamsFrom(parameters);

                    if (!client.BaseAddress.AbsoluteUri.Contains("?"))
                    {
                        parameterString = "?" + parameterString;
                    }
                    response = client.GetAsync(parameterString).Result;
                }


                string result = response.Content.ReadAsStringAsync().Result;

                IEnumerable<string> values;
                string str = string.Empty;

                if (callback == null)
                {
                    return;
                }
                if (!response.IsSuccessStatusCode) //Non-200
                {
                    callback(new Exception("Non 200 response"));
                    return;
                }
                if (response.Headers.TryGetValues("x-error", out values))
                {
                    str = values.FirstOrDefault();
                    Exception exception = new Exception("X-Error Detected: " + str);
                    callback(exception);
                    return;
                }

                callback(null);
            }
            catch (HttpRequestException e)
            {
                if (callback == null)
                {
                    return;
                }
                callback(e);
            }
        }

        internal string EncodedParamsFrom(Dictionary<string, object> payload)
        {

            if (payload == null)
            {
                return "";
            }

            string queryString = "";

            SortedDictionary<string, object> sortedPayload = new SortedDictionary<string, object>(payload);

            foreach (KeyValuePair<string, object> entry in sortedPayload)
            {
                string key = entry.Key;
                object value = entry.Value;

                if (string.IsNullOrEmpty(key) || value == null)
                {
                    continue;
                }

                if (queryString.Length > 1)
                {
                    queryString += "&";
                }
                string[] arrayValue;
                if ((arrayValue = Utils.CoerceIntoStringArray(value)) != null)
                {
                    queryString += (System.Net.WebUtility.UrlEncode(key) + "=");
                    queryString += EncodeArrayParams(arrayValue);
                }
                else
                {
                    queryString += (System.Net.WebUtility.UrlEncode(key) + "=");
                    queryString += System.Net.WebUtility.UrlEncode(value.ToString());
                }
            }

            return queryString;
        }

        internal string EncodeArrayParams(string[] arrayParams)
        {
            StringBuilder builder = new StringBuilder("[");

            for (int i = 0; i < arrayParams.Length; i++)
            {
                builder.Append(arrayParams[i]);
                if (i < arrayParams.Length - 1)
                {
                    builder.Append(",");
                }
            }

            builder.Append("]");

            return System.Net.WebUtility.UrlEncode(builder.ToString());
        }
    }
}
