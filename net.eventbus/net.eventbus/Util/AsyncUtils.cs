#region

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

#endregion

namespace net.eventbus.Util
{
    internal static class AsyncUtils
    {
        /// <summary>
        ///     Call a blocking function via <code>func</code> parameter on an own thread and at the end call
        ///     <code>finishedCallback</code> on application thread to return the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dispatcher">Target dispatcher to run second action callback</param>
        /// <param name="func"></param>
        /// <param name="finishedCallback"></param>
        public static void RunAsynchronious<T>(Dispatcher dispatcher, Func<T> func, Action<T> finishedCallback)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                var result = func.Invoke();
                dispatcher.Invoke(() => finishedCallback.Invoke(result));
            });
        }

        /// <summary>
        ///     Call a blocking function via <code>func</code> parameter on an own thread
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        public static void RunAsynchronious(Action func)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                try
                {
                    func.Invoke();
                }
                catch (TaskCanceledException)
                {
                }
            });
        }
    }
}