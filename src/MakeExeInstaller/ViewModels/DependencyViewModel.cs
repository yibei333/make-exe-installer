using MakeExeInstaller.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace MakeExeInstaller.ViewModels
{
    public class DependencyViewModel : PageViewModelBase
    {
        public DependencyViewModel(MainViewModel mainViewModel) : base(mainViewModel)
        {
            Dependencies = new ObservableCollection<IDependency>
            {
                new Webview2Dependency()
            };
            InstallCommand = new AsyncRelayCommand(InstallDependencies);
        }

        public ObservableCollection<IDependency> Dependencies { get; }

        public AsyncRelayCommand InstallCommand { get; }

        async Task InstallDependencies(object parameter)
        {
            MainViewModel.CloseWindowButtonEnabled = false;
            foreach (var dependency in Dependencies)
            {
                await dependency.Command.ExecuteAsync(null);
                if (dependency.State != DependencyState.Completed)
                {
                    MessageBox.Show(dependency.Message);
                    MainViewModel.CloseWindowButtonEnabled = true;
                    return;
                }
            }
            MainViewModel.CloseWindowButtonEnabled = true;
            MainViewModel.Index++;
        }
    }

    public enum DependencyState
    {
        Waiting,
        Running,
        Completed,
        Error
    }

    public interface IDependency : INotifyPropertyChanged
    {
        string Name { get; }

        DependencyState State { get; }

        string Message { get; }

        AsyncRelayCommand Command { get; }
    }

    public class Webview2Dependency : IDependency
    {
        public event PropertyChangedEventHandler PropertyChanged;
        DependencyState _state;
        string _message;

        public Webview2Dependency()
        {
            Command = new AsyncRelayCommand(InstallWebView2);
        }

        public string Name => "WebView2运行时";

        public DependencyState State
        {
            get => _state;
            private set
            {
                if (_state == value) return;
                _state = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(State)));
            }
        }

        public string Message
        {
            get => _message;
            private set
            {
                if (_message == value) return;
                _message = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Message)));
            }
        }

        public AsyncRelayCommand Command { get; }

        async Task InstallWebView2(object parameter)
        {
            State = DependencyState.Running;
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream($"Data/Dependencies/EnsureWebview2RuntimeInstalled.exe");
            if (stream is null)
            {
                State = DependencyState.Error;
                Message = "找不到依赖资源文件:EnsureWebview2RuntimeInstalled.exe";
                return;
            }
            var path = AppDomain.CurrentDomain.BaseDirectory + "EnsureWebview2RuntimeInstalled.exe";
            var targetStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            stream.CopyTo(targetStream);
            targetStream.Flush();
            targetStream.Dispose();
            stream.Dispose();

            var process = new Process
            {
                StartInfo = new ProcessStartInfo(path)
            };
            process.Start();
            await Task.Run(() => process.WaitForExit()).ContinueWith((_) =>
            {
                if (process.ExitCode == 0)
                {
                    State = DependencyState.Completed;
                }
                else
                {
                    State = DependencyState.Error;
                    Message = "安装失败";
                }
                if (File.Exists(path)) File.Delete(path);
                var downloadPath = AppDomain.CurrentDomain.BaseDirectory + "MicrosoftEdgeWebView2RuntimeInstallerX64.exe";
                if (File.Exists(downloadPath)) File.Delete(downloadPath);
            });
        }
    }
}
