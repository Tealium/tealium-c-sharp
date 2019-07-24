using NUnit.Framework;
using System;
using TealiumCSharp;

// INTEGRATION TESTS ... REALLY

namespace Tests
{
    [TestFixture()]
    public class TestsTealium
    {
        Config testConfig = new Config("test", "test", "test");

        [Test()]
        public void TestSettingTraceId()
        {
            Tealium instance = new Tealium(testConfig);

            Assert.IsNull(instance.TraceId);

            instance.JoinTrace("12345");
            Assert.AreEqual("12345", instance.TraceId);
        }

        [Test()]
        public void TestLeavingTrace()
        {
            Tealium instance = new Tealium(testConfig);

            Assert.IsNull(instance.TraceId);

            instance.JoinTrace("12345");
            Assert.AreEqual("12345", instance.TraceId);

            instance.LeaveTrace();
            Assert.IsNull(instance.TraceId);
        }

        [Test()]
        public void TestKillingTrace()
        {
            Tealium instance = new Tealium(testConfig);

            Assert.IsNull(instance.TraceId);

            instance.JoinTrace("12345");
            Assert.AreEqual("12345", instance.TraceId);

            instance.KillTraceSession();
            //System.Threading.Thread.Sleep(150);
            Assert.IsNull(instance.TraceId);
        }
    }
}
