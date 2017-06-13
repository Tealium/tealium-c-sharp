using NUnit.Framework;
using System;
using TealiumCSharp;

namespace Tests
{
	[TestFixture()]
	public class TestsTealiumModule
	{
		TestsHelper utils = new TestsHelper();

		[Test()]
		public void TestsPublicCommandsReturn()
		{
			LoggerModule module = new LoggerModule();
			ModuleResponses responses = utils.moduleReturnsFromAllBaseCalls(module);

			Assert.True(responses.Enabled == true, "Failed to enable.");
			Assert.True(responses.Disabled == true, "Failed to disable.");
			Assert.True(responses.Tracked == true, "Failed to track.");
			Assert.True(responses.Reported == true, "Failed to handle report.");
		}

		[Test()]
		public void TestCompareTo()
		{
			Module moduleA = new Module();
			moduleA.NameId = "testA";
			Module moduleB = new Module();
			moduleB.NameId = "testB";
			Module moduleC = new Module();
			moduleC.NameId = "testA";
			Assert.True(moduleA.CompareTo(moduleB) == -1);
			Assert.True(moduleA.CompareTo(moduleC) == 0);
			Assert.True(moduleB.CompareTo(moduleA) == 1);
		}

	}
}
