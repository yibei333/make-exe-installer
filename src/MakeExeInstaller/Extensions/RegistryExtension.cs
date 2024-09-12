﻿using Microsoft.Win32;

namespace MakeExeInstaller.Extensions
{
    public static class RegistryExtension
    {
        static readonly string SubKeyPath = $"Software\\{App.Config.Name}";
        static readonly string UninstallKeyPath = $"Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{App.Config.Name}";
        static readonly string KeyName = "InstallLocation";

        public static void Set(string location)
        {
            var key = Registry.CurrentUser.CreateSubKey(SubKeyPath);

            if (key != null)
            {
                key.SetValue(KeyName, location);
                key.Close();
                SetSystemInformation(location);
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
            Registry.CurrentUser.DeleteSubKey(UninstallKeyPath);
        }

        static void SetSystemInformation(string location)
        {
            var key = Registry.CurrentUser.CreateSubKey(UninstallKeyPath);

            if (key != null)
            {
                key.SetValue("DisplayIcon", location.CombinePath(App.Config.ExePath));
                key.SetValue("DisplayName", App.Config.DisplayName);
                key.SetValue("DisplayVersion", App.Config.Version);
                key.SetValue("InstallLocation", location);
                key.SetValue("NoModify", 1);
                key.SetValue("NoRepair", 1);
                key.SetValue("Publisher", App.Config.Publisher);
                key.SetValue("UninstallString", location.CombinePath("Uninstall.exe"));
                key.Close();
            }
        }

        public static void SetSystemSizeInformation(int size)
        {
            var key = Registry.CurrentUser.CreateSubKey(UninstallKeyPath);

            if (key != null)
            {
                key.SetValue("EstimatedSize", size, RegistryValueKind.DWord);
                key.Close();
            }
        }
    }
}
