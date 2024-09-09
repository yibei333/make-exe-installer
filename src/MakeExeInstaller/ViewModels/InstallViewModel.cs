﻿using MakeExeInstaller.Mvvm;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;

namespace MakeExeInstaller.ViewModels
{
    public class InstallViewModel : PageViewModelBase
    {
        public InstallViewModel(MainViewModel mainViewModel) : base(mainViewModel)
        {
            //var assembly = Assembly.GetExecutingAssembly();
            //var xx = assembly.GetManifestResourceNames();
            //foreach (var x in xx)
            //{
            //    var y = assembly.GetManifestResourceInfo(x);
            //    var stream = assembly.GetManifestResourceStream(x);
            //}

            //var a = Process.GetProcessesByName("SharpDevTool").FirstOrDefault();
            //a?.Kill();
            //a?.Dispose();

            //appShortcutToDesktop("test");
        }

        //private void appShortcutToDesktop(string linkName)
        //{
        //    IShellLink link = (IShellLink)new ShellLink();

        //    // setup shortcut information
        //    link.SetDescription($"{linkName} description");
        //    string app = Assembly.GetExecutingAssembly().Location;
        //    link.SetPath(app);

        //    // save it
        //    IPersistFile file = (IPersistFile)link;
        //    string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        //    file.Save(Path.Combine(desktopPath, $"{linkName}.lnk"), false);
        //}

        //[ComImport]
        //[Guid("00021401-0000-0000-C000-000000000046")]
        //internal class ShellLink
        //{
        //}

        //[ComImport]
        //[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        //[Guid("000214F9-0000-0000-C000-000000000046")]
        //internal interface IShellLink
        //{
        //    void GetPath([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxPath, out IntPtr pfd, int fFlags);
        //    void GetIDList(out IntPtr ppidl);
        //    void SetIDList(IntPtr pidl);
        //    void GetDescription([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName, int cchMaxName);
        //    void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);
        //    void GetWorkingDirectory([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cchMaxPath);
        //    void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);
        //    void GetArguments([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cchMaxPath);
        //    void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
        //    void GetHotkey(out short pwHotkey);
        //    void SetHotkey(short wHotkey);
        //    void GetShowCmd(out int piShowCmd);
        //    void SetShowCmd(int iShowCmd);
        //    void GetIconLocation([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath, int cchIconPath, out int piIcon);
        //    void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);
        //    void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, int dwReserved);
        //    void Resolve(IntPtr hwnd, int fFlags);
        //    void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
        //}
    }
}