namespace net.eventbus.test
{
    public sealed class EventBusTestClass
    {
        public int ParameterStringCalls { get; private set; }
        public int ParameterIntegerCalls { get; private set; }
        public int ParameterStringWithTestCalls { get; private set; }

        [Subscribe]
        private void OnParameterString(string value)
        {
            ParameterStringCalls++;
        }

        [Subscribe]
        private void OnParameterInteger(int value)
        {
            ParameterIntegerCalls++;
        }

        [Subscribe(Name = "Test")]
        private void OnParameterStringWithTest(string value)
        {
            ParameterStringWithTestCalls++;
        }
    }
}