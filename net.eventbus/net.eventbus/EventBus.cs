#region

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using net.eventbus.Type;
using net.eventbus.Util;

#endregion

namespace net.eventbus
{
    public sealed class EventBus
    {
        private readonly IDictionary<object, IDictionary<MethodKey, IList<MethodValue>>> _instanceDict =
            new Dictionary<object, IDictionary<MethodKey, IList<MethodValue>>>();

        private EventBus()
        {
        }

        public static EventBus Instance { get; } = new EventBus();

        public void Register(object instance)
        {
            if (instance == null)
            {
                throw new ArgumentException("instance cannot be null");
            }

            if (_instanceDict.ContainsKey(instance.GetType()))
            {
                return;
            }

            var methodDict = new Dictionary<MethodKey, IList<MethodValue>>();

            foreach (var method in instance.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                var customAttributes = method.GetCustomAttributes(typeof(SubscribeAttribute), true);
                if (customAttributes.Length <= 0)
                {
                    continue;
                }
                if (customAttributes.Length > 1)
                {
                    throw new InvalidProgramException(
                        "The attribute for subscribing can exists only one times per method; method: " +
                        method.DeclaringType?.FullName + "#" + method.Name);
                }
                if (method.GetParameters().Length != 1)
                {
                    throw new InvalidProgramException("A subscribe method must have only one parameter; method: " +
                                                      method.DeclaringType?.FullName + "#" + method.Name);
                }

                var subscribe = (SubscribeAttribute) customAttributes[0];

                var key = new MethodKey(subscribe.Name, method.GetParameters()[0].ParameterType);
                if (!methodDict.ContainsKey(key))
                {
                    methodDict.Add(key, new List<MethodValue>());
                }

                methodDict[key].Add(new MethodValue(method, subscribe));
            }

            _instanceDict.Add(instance, methodDict);
        }

        public void Unregister(object instance)
        {
            if (instance == null)
            {
                throw new ArgumentException("instance cannot be null");
            }

            if (!_instanceDict.ContainsKey(instance))
            {
                return;
            }

            _instanceDict.Remove(instance);
        }

        public void Fire(object value)
        {
            Fire(null, value);
        }

        public void Fire(string name, object value)
        {
            var key = new MethodKey(name, value.GetType());

            foreach (var pair in _instanceDict)
            {
                if (!pair.Value.ContainsKey(key))
                {
                    continue;
                }

                foreach (var method in pair.Value[key])
                {
                    switch (method.Subscribe.ThreadMode)
                    {
                        case ThreadMode.Default:
                            method.Method.Invoke(pair.Key, new[] {value});
                            break;
                        case ThreadMode.Background:
                            AsyncUtils.RunAsynchronious(() =>
                            {
                                try
                                {
                                    method.Method.Invoke(pair.Key, new[] {value});
                                }
                                catch (TargetInvocationException e)
                                {
                                    if (e.InnerException is TaskCanceledException)
                                    {
                                        return;
                                    }

                                    throw;
                                }
                            });
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }
        }
    }

    #region Subscribe Attribute

    public enum ThreadMode
    {
        Default,
        Background
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class SubscribeAttribute : Attribute
    {
        public string Name { get; set; }
        public ThreadMode ThreadMode { get; set; }
    }

    #endregion
}