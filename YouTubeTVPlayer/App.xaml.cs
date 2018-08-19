using System.Reflection;
using System.Threading;
using System.Windows;

namespace Zabrodin.YouTubeTVPlayer
{
    public partial class App
    {
        private Mutex mutex;

        protected override void OnStartup(StartupEventArgs e)
        {
            string mutexName = Assembly.GetExecutingAssembly().GetType().GUID.ToString();
            mutex = new Mutex(false, mutexName, out bool createdNew);
            if (!createdNew)
            {
                Shutdown();
                return;
            }

            base.OnStartup(e);
        }
    }
}
