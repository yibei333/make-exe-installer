using MakeExeInstaller.Extensions;
using System.Diagnostics;
using System;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Windows;

namespace MakeExeInstaller
{
    public partial class App : Application
    {
        public App()
        {
            RequestAdminUAC();
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;

            //todo:modify taskbar name
            //todo:add uninstall
            //sign
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
        }

        public static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();
        public static string AssemblyName { get; } = Assembly.GetName().Name;
        public static AppConfig Config { get; } = GetConfig();

        static AppConfig GetConfig()
        {
            var stream = Assembly.GetManifestResourceStream("Data/config.json");
            var bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Dispose();
            var json = Encoding.UTF8.GetString(bytes);
            return json.DeSerialize<AppConfig>();
        }

        static void RequestAdminUAC()
        {
            var wi = WindowsIdentity.GetCurrent();
            var wp = new WindowsPrincipal(wi);

            bool runAsAdmin = wp.IsInRole(WindowsBuiltInRole.Administrator);

            if (!runAsAdmin)
            {
                var processInfo = new ProcessStartInfo(Assembly.GetExecutingAssembly().CodeBase);
                processInfo.UseShellExecute = true;
                processInfo.Verb = "runas";

                try
                {
                    Process.Start(processInfo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                Environment.Exit(0);
            }
        }
    }

    public class AppConfig
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string ExePath { get; set; }
        public string Version { get; set; }
        public string Publisher { get; set; }
    }
}
