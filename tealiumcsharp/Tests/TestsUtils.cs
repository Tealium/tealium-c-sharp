using NUnit.Framework;
using System.Collections.Generic;
using TealiumCSharp;

namespace Tests
{
	[TestFixture()]
	public class TestsUtils
	{


		[Test()]
		public void TestsDictionaryMergingStrings()
		{

			Dictionary<string, object> dict1 = new Dictionary<string, object>();
			dict1.Add("a", "b");

			Dictionary<string, object> dict2 = new Dictionary<string, object>();
			dict2.Add("a", "c");
			dict2.Add("1", "2");

			Dictionary<string, object> dictFinal = Utils.MergeDictionary(dict1, dict2);

			string text = "";
			foreach (KeyValuePair<string, object> kvp in dictFinal)
			{
				text += string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
			}

			System.Diagnostics.Debug.WriteLine("dictFinal: " + text);
			Dictionary<string, object> expectedDict = new Dictionary<string, object>();
			expectedDict.Add("a","c");
			expectedDict.Add("1","2");

			bool areEqual = TestsHelper.AreDictionariesEqual(expectedDict, dictFinal);
			Assert.True(areEqual == true, "Unexpected dictionary returned: " + dictFinal.ToString());
		}

		[Test()]
		public void TestsDictionaryMergingStringArrays()
		{

			Dictionary<string, object> dict1 = new Dictionary<string, object>();
			string[] array1 = { "a", "b", "c" };
			dict1.Add("a", array1);

			Dictionary<string, object> dict2 = new Dictionary<string, object>();
			string[] array2 = { "1", "2", "3" };
			dict2.Add("a", array2);

			Dictionary<string, object> dict1Cleaned = Utils.Sanitized(dict1);
			Dictionary<string, object> dict2Cleaned = Utils.Sanitized(dict2);

			Dictionary<string, object> dictFinal = Utils.MergeDictionary(dict1Cleaned, dict2Cleaned);
			Dictionary<string, object> expectedDict = new Dictionary<string, object>();
			string[] arrayExpected = { "1", "2", "3" };
			expectedDict.Add("a", arrayExpected);

			string dict2String = "";
			foreach (KeyValuePair<string, object> kvp in dict2)
			{
				dict2String += string.Format("\n\tKey = {0}, Value = {1}", kvp.Key, kvp.Value);
			}
			string dictFinalString = "";
			foreach (KeyValuePair<string, object> kvp in dictFinal)
			{
				dictFinalString += string.Format("\n\tKey = {0}, Value = {1}", kvp.Key, kvp.Value);
			}
			string expectedDictString = "";
			foreach (KeyValuePair<string, object> kvp in expectedDict)
			{
				expectedDictString += string.Format("\n\tKey = {0}, Value = {1}", kvp.Key, kvp.Value);
			}

			bool areEqual = TestsHelper.AreDictionariesEqual(expectedDict, dict2);
			Assert.True(areEqual == true, "Unexpected dictionary returned: " + dict2String + "\nexpectedDict:" + expectedDictString);
		}

		[Test()]
		public void TestsDictionaryMergingMixedValues()
		{

			Dictionary<string, object> dict1 = new Dictionary<string, object>();
			dict1.Add("a", "b");

			Dictionary<string, object> dict2 = new Dictionary<string, object>();
			dict2.Add("alpha2", "!@#$%^&*()");
			int[] array2 = { 1, 2, 3 };
			dict2.Add("a", array2);


			Dictionary<string, object> dictFinal = Utils.MergeDictionary(dict1, dict2);
			Dictionary<string, object> expectedDict = new Dictionary<string, object>();
			expectedDict.Add("alpha2", "!@#$%^&*()");
			int[] arrayExpected = { 1, 2, 3 };
			expectedDict.Add("a", arrayExpected);

			string dictFinalString = "";
			foreach (KeyValuePair<string, object> kvp in dictFinal)
			{
				dictFinalString += string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
			}

			bool areEqual = TestsHelper.AreDictionariesEqual(expectedDict, dictFinal);
			Assert.True(areEqual == true, "Unexpected dictionary returned: " + dictFinalString);
		}

	}


}
