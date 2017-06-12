using NUnit.Framework;
using System;
using TealiumCSharp;
using System.Collections.Generic;


namespace Tests
{
	[TestFixture()]
	public class TestsCollectModule
	{

		TestsHelper utils = new TestsHelper();

		[Test()]
		public void TestsPublicCommandsReturn()
		{ 
			CollectModule module = new CollectModule();
			ModuleResponses responses = utils.moduleReturnsFromAllBaseCalls(module);

			Assert.True(responses.Enabled, "Failed to enable.");
			Assert.True(responses.Disabled, "Failed to disable.");
			Assert.True(responses.Tracked, "Failed to track.");
			Assert.True(responses.Reported, "Failed to handle report.");

		}
	}
}
