#region

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace net.eventbus.test
{
    [TestClass]
    public class EventBusTest
    {
        [TestMethod]
        public void Test()
        {
            var testClass = new EventBusTestClass();
            EventBus.Instance.Register(testClass);

            Assert.AreEqual(0, testClass.ParameterIntegerCalls);
            Assert.AreEqual(0, testClass.ParameterStringCalls);
            Assert.AreEqual(0, testClass.ParameterStringWithTestCalls);

            EventBus.Instance.Fire(1d);

            Assert.AreEqual(0, testClass.ParameterIntegerCalls);
            Assert.AreEqual(0, testClass.ParameterStringCalls);
            Assert.AreEqual(0, testClass.ParameterStringWithTestCalls);

            EventBus.Instance.Fire("abc");

            Assert.AreEqual(0, testClass.ParameterIntegerCalls);
            Assert.AreEqual(1, testClass.ParameterStringCalls);
            Assert.AreEqual(0, testClass.ParameterStringWithTestCalls);

            EventBus.Instance.Fire(123);

            Assert.AreEqual(1, testClass.ParameterIntegerCalls);
            Assert.AreEqual(1, testClass.ParameterStringCalls);
            Assert.AreEqual(0, testClass.ParameterStringWithTestCalls);

            EventBus.Instance.Fire("Test", "abc");

            Assert.AreEqual(1, testClass.ParameterIntegerCalls);
            Assert.AreEqual(1, testClass.ParameterStringCalls);
            Assert.AreEqual(1, testClass.ParameterStringWithTestCalls);

            EventBus.Instance.Unregister(testClass);

            //After check
            EventBus.Instance.Fire(1d);

            Assert.AreEqual(1, testClass.ParameterIntegerCalls);
            Assert.AreEqual(1, testClass.ParameterStringCalls);
            Assert.AreEqual(1, testClass.ParameterStringWithTestCalls);

            EventBus.Instance.Fire("abc");

            Assert.AreEqual(1, testClass.ParameterIntegerCalls);
            Assert.AreEqual(1, testClass.ParameterStringCalls);
            Assert.AreEqual(1, testClass.ParameterStringWithTestCalls);

            EventBus.Instance.Fire(123);

            Assert.AreEqual(1, testClass.ParameterIntegerCalls);
            Assert.AreEqual(1, testClass.ParameterStringCalls);
            Assert.AreEqual(1, testClass.ParameterStringWithTestCalls);

            EventBus.Instance.Fire("Test", "abc");

            Assert.AreEqual(1, testClass.ParameterIntegerCalls);
            Assert.AreEqual(1, testClass.ParameterStringCalls);
            Assert.AreEqual(1, testClass.ParameterStringWithTestCalls);
        }
    }
}