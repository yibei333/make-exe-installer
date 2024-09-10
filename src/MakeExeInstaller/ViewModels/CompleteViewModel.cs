using MakeExeInstaller.Extensions;
using MakeExeInstaller.Mvvm;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace MakeExeInstaller.ViewModels
{
    public class CompleteViewModel : PageViewModelBase
    {
        bool _run = true;

        public CompleteViewModel(MainViewModel mainViewModel) : base(mainViewModel)
        {
            CompleteCommand = new AsyncRelayCommand(Complete);
        }

        public bool Run
        {
            get => _run;
            set
            {
                if (_run == value) return;
                _run = value;
                OnPropertyChanged(nameof(Run));
            }
        }

        public AsyncRelayCommand CompleteCommand { get; }

        async Task Complete(object parameter)
        {
            if (Run)
            {
                var exe = RegistryExtension.Get().CombinePath(App.Config.Name).CombinePath("bin").CombinePath(App.Config.ExePath);
                if (File.Exists(exe))
                {
                    var process = new Process
                    {
                        StartInfo = new ProcessStartInfo(exe)
                    };
                    process.Start();
                    process.WaitForExit();
                }

                Application.Current.Shutdown();
                await Task.CompletedTask;
            }
        }
    }
}
