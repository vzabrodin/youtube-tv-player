using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Zabrodin.YouTubeTVPlayer
{
    public static class DispatcherHelper
    {
        private static Dispatcher Dispatcher => Application.Current.Dispatcher;

        public static void ExecuteWithCheck(Action method, bool async = false)
        {
            if (Dispatcher.CheckAccess())
            {
                method();
            }
            else
            {
                if (async)
                {
                    Dispatcher.BeginInvoke(method);
                }
                else
                {
                    Dispatcher.Invoke(method);
                }
            }
        }

        public static void Execute(Action method)
            => Dispatcher.Invoke(method);

        public static Task ExecuteAsync(Action method)
            => Dispatcher.BeginInvoke(method).Task;

        public static void ExecuteWithWait(Func<Task> method)
        {
            Task task = null;
            Dispatcher.Invoke(() => { task = method.Invoke(); });
            task.Wait();
        }

        public static Task<TResult> ExecuteAsync<TResult>(Func<TResult> method)
        {
            return Task.Run(async () =>
            {
                TResult result = default(TResult);
                await Dispatcher.BeginInvoke((Action)(() => result = method())).Task;

                return result;
            });
        }

        public static T Execute<T>(Func<T> method)
            => Dispatcher.Invoke(method);

        public static T ExecuteWithCheck<T>(Func<T> method)
            => Dispatcher.CheckAccess() ? method() : Execute(method);
    }
}
