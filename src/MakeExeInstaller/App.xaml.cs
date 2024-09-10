using MakeExeInstaller.Extensions;
using System.Reflection;
using System.Text;
using System.Windows;

namespace MakeExeInstaller
{
    public partial class App : Application
    {
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
    }

    public class AppConfig
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string ExePath { get; set; }
        public string Version { get; set; }
    }
}
