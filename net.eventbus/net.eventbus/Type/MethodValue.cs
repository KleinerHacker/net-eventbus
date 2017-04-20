#region

using System.Reflection;

#endregion

namespace net.eventbus.Type
{
    internal sealed class MethodValue
    {
        public MethodValue(MethodInfo method, SubscribeAttribute subscribe)
        {
            Method = method;
            Subscribe = subscribe;
        }

        public MethodInfo Method { get; }
        public SubscribeAttribute Subscribe { get; }

        public override string ToString()
        {
            return $"{nameof(Method)}: {Method}, {nameof(Subscribe)}: {Subscribe}";
        }
    }
}