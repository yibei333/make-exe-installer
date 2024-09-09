using MakeExeInstaller.Mvvm;
using System.Diagnostics;
using System.Reflection;

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
        }
    }
}
