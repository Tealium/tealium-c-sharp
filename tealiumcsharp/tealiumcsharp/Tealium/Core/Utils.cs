using System;
using System.Collections.Generic;
using System.Linq;

namespace TealiumCSharp
{
    public class Utils
    {
        public Utils()
        {
        }

        public static Dictionary<string, object> MergeDictionary(Dictionary<string, object> dest,
                                                                 Dictionary<string, object> data)
        {

            foreach (KeyValuePair<string, object> kvp in data)
            {
                object value;
                if (!dest.TryGetValue(kvp.Key, out value))
                {
                    dest.Add(kvp.Key, kvp.Value);
                }
                else
                {
                    dest[kvp.Key] = kvp.Value;
                }

            }
            return dest;
        }

        public static Dictionary<string, object> Sanitized(Dictionary<string, object> original)
        {
            string[] arrayValue;
            Dictionary<string, object> cleanCopy = new Dictionary<string, object>();

            foreach (KeyValuePair<string, object> kvp in original)
            {
                string key = kvp.Key;
                object val = kvp.Value;

                if (key == null || val == null)
                {
                    continue;
                }

                if ((arrayValue = CoerceIntoStringArray(val)) == null)
                {
                    cleanCopy.Add(kvp.Key, kvp.Value);
                }
                else
                {
                    cleanCopy.Add(kvp.Key, arrayValue);
                }

            }

            return cleanCopy;
        }

        public static string[] CoerceIntoStringArray(object source)
        {
            if (!source.GetType().IsArray)
            {
                return null;
            }

            return CoerceArray(source);
        }

        static string[] CoerceArray(object array)
        {
            if (array == null | !array.GetType().IsArray)
            {
                return null;
            }

            string[] copy = new string[((Array)array).Length];
            int count = 0;
            object val;

            for (int i = 0; i < copy.Length; i++)
            {
                if ((val = ((Array)array).GetValue(i)) != null)
                {
                    copy[count++] = val.ToString();
                }
            }

            if (copy.Length != count)
            {
                Array.Copy(copy, copy, count);
            }

            return copy;
        }

        public static String DictionaryToJsonString(Dictionary<String, object> data)
        {
            var entries = data.Select(item =>
                string.Format("{0}:{1}", ToJsonString(item.Key), item.Value is Dictionary<string, object> ? DictionaryToJsonString((Dictionary<string, object>)item.Value) : (item.Value.GetType().IsArray ? ToJsonArray((string[])item.Value) : ToJsonString((String)item.Value)))
            );
            return "{" + string.Join(",", entries) + "}";
        }

        static String ToJsonString(string str)
        {
            return string.Format("\"{0}\"", str);
        }

        static String ToJsonArray(string[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array.SetValue(ToJsonString((String)array.GetValue(i)), i);
            }
            string result = "[" + string.Join(",", array) + "]";
            return result;
        }
    }
}
