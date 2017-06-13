using NUnit.Framework;
using System;
using System.Collections.Generic;
using TealiumCSharp;
namespace Tests
{
	[TestFixture()]
	public class TestsCollect
	{
		Config config = new Config("test",
											   "test",
											   "test",
											   "test",
											   new string[] {
											"TealiumCSharp.AppDataModule",
											"TealiumCSharp.CollectModule",
											"TealiumCSharp.LoggerModule"
												},
											   null,
											   null);

		Dictionary<string, object> StringStringDict()
		{
			SortedDictionary<string, object> sorted = new SortedDictionary<string, object>
			{
				{ "key", "value"},
				{ "key1", "value1"},
				{ "key_2", "value_2"},
				{ "a key", "a value"},
				{ "specialKey", "$-_.+!*'(),"}
			};

			Dictionary<string, object> sortedStringStringDictionary = new Dictionary<string, object>(sorted);

			return sortedStringStringDictionary;
		}

		Dictionary<string, object> StringArrayDict()
		{

			SortedDictionary<string, object> sorted = new SortedDictionary<string, object>
			{
				{"array1", new string[] {"foo", "bar", "", "foobar,\\r\\n\\tand some extra stuff"}},
				{"array2", new string[] { "123", "true", "!@#$%^&*()_+"}}
			};


			Dictionary<string, object> sortedStringArrayDictionary = new Dictionary<string, object>(sorted);

			return sortedStringArrayDictionary;
		}

		Dictionary<string, object> StringMixedAcceptableArrayDict()
		{
			SortedDictionary<string, object> sorted = new SortedDictionary<string, object>
			{
				{ "key_2", "value_2"},
				{ "array1", new string[] { "foo", "bar", "", "foobar,\\r\\n\\tand some extra stuff" }}
			};

			Dictionary<string, object> sortedStringMixedDictionary = new Dictionary<string, object>(sorted);

			return sortedStringMixedDictionary;
		}

		static string EXPECTED_STRINGSTRING_QSP = "a+key=a+value&key=value&key_2=value_2&key1=value1&specialKey=%24-_.%2B!*%27()%2C";
		static string EXPECTED_STRINGARRAY_QSP = "array1=%5Bfoo%2Cbar%2C%2Cfoobar%2C%5Cr%5Cn%5Ctand+some+extra+stuff%5D&array2=%5B123%2Ctrue%2C!%40%23%24%25%5E%26*()_%2B%5D";
		static string EXPECTED_STRINGMIXED_QSP = "array1=%5Bfoo%2Cbar%2C%2Cfoobar%2C%5Cr%5Cn%5Ctand+some+extra+stuff%5D&key_2=value_2";

		[Test()]
		public void TestsEncodedStringStringParamsFrom()
		{
			Collect collect = new Collect(config, Collect.DefaultBaseURLString());

			string stringStringParams = collect.EncodedParamsFrom(StringStringDict());

			Assert.True(EXPECTED_STRINGSTRING_QSP.Equals(stringStringParams));

		}

		[Test()]
		public void TestsEncodedStringArrayParamsFrom()
		{
			Collect collect = new Collect(config, Collect.DefaultBaseURLString());

			string stringArrayParams = collect.EncodedParamsFrom(StringArrayDict());

			Assert.True(EXPECTED_STRINGARRAY_QSP.Equals(stringArrayParams));

		}

		[Test()]
		public void TestsEncodedStringMixedParamsFrom()
		{
			Collect collect = new Collect(config, Collect.DefaultBaseURLString());

			string stringMixedParams = collect.EncodedParamsFrom(StringMixedAcceptableArrayDict());

			Assert.True(EXPECTED_STRINGMIXED_QSP.Equals(stringMixedParams));

		}

		static string EXPECTED_ENCODED_ARRAY_PARAMS = "%5B123%2Ctrue%2C!%40%23%24%25%5E%26*()_%2B%5D";

		[Test()]
		public void TestsEncodeArrayParams()
		{
			Collect collect = new Collect(config, Collect.DefaultBaseURLString());

			string[] stringArray = new string[] { "123", "true", "!@#$%^&*()_+" };

			string encodedStringArray = collect.EncodeArrayParams(stringArray);

			Assert.True(EXPECTED_ENCODED_ARRAY_PARAMS.Equals(encodedStringArray));

		}

	}
}