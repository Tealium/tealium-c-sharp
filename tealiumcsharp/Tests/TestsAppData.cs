using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TealiumCSharp;


namespace Tests
{
	[TestFixture()]
	public class TestsAppData
	{
		
		[Test()]
		public void TestsAddAndGetData()
		{
			AppData appData = new AppData();
			Dictionary<string, object> testSubDictionary = new Dictionary<string, object>();
			testSubDictionary.Add("key","value");

			appData.AddData(testSubDictionary);
			appData.AddData(testSubDictionary);

			Dictionary<string, object> appDataDictionary = appData.GetData();

			Assert.True(TestsHelper.DictionaryContainsSubDictionary(appDataDictionary, testSubDictionary));

		}

		[Test()]
		public void TestsGetRandom()
		{
			AppData appData = new AppData();
			string[] randomArray = new string[100];

			Regex regex = new Regex("^[0-9]{16}$");

			for (int i = 0; i < randomArray.Length; i++)
			{
				string random = appData.GetRandom();
				System.Threading.Thread.Sleep(30);
				if (randomArray.Contains(random))
				{
					Assert.Fail("Duplicate random number");
				}
				else
				{
					Match match = regex.Match(random);
					if (!match.Success)
					{
						Assert.Fail("Random number is not 16 digits");
					}
					randomArray[i] = random;
				}
			}
		}

		[Test()]
		public void TestsSetSessionId()
		{
			AppData appData = new AppData();
			string expectedSessionIdValue = "1112223334444";

			appData.SetSessionId(expectedSessionIdValue);

			Dictionary<string, object> data = appData.GetData();

			Assert.True(data[AppDataConstants.SESSION_ID].ToString() == expectedSessionIdValue);


		}

		[Test()]
		public void TestsNewSessionId()
		{
			AppData appData = new AppData();

			string sessionId1 = appData.NewSessionId();
			System.Threading.Thread.Sleep(10);
			string sessionId2 = appData.NewSessionId();

			Assert.True(!sessionId1.Equals(sessionId2));
		}

	}
}
