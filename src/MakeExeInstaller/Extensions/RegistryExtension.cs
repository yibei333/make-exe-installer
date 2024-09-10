using Microsoft.Win32;

namespace MakeExeInstaller.Extensions
{
    public static class RegistryExtension
    {
        static readonly string SubKeyPath = $"Software\\{App.Config.Name}";
        static readonly string KeyName = "InstallLocation";

        public static void Set(string location)
        {
            var key = Registry.CurrentUser.CreateSubKey(SubKeyPath);

            if (key != null)
            {
                key.SetValue(KeyName, location);
                key.Close();
            }
        }

        public static string Get()
        {
            var key = Registry.CurrentUser.OpenSubKey(SubKeyPath);
            return key?.GetValue(KeyName)?.ToString();
        }

        public static void Delete()
        {
            Registry.CurrentUser.DeleteSubKey(SubKeyPath);
        }
    }
}
