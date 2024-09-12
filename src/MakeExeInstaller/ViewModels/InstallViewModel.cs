using MakeExeInstaller.Extensions;
using MakeExeInstaller.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MakeExeInstaller.ViewModels
{
    public class InstallViewModel : PageViewModelBase
    {
        string _targetPath;

        public InstallViewModel(MainViewModel mainViewModel) : base(mainViewModel)
        {
            TargetPath = RegistryExtension.Get();
            if (TargetPath.IsNullOrWhiteSpace()) TargetPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            SelectPathCommand = new AsyncRelayCommand(SelectPath);
            InstallCommmand = new AsyncRelayCommand(Install);
        }

        public string TargetPath
        {
            get => _targetPath;
            set
            {
                if (_targetPath == value) return;
                _targetPath = value;
                OnPropertyChanged(nameof(TargetPath));
            }
        }

        public string ExePath => TargetPath.CombinePath(App.Config.ExePath);
        public string FileListPath => TargetPath.CombinePath("app.file.list");

        public AsyncRelayCommand SelectPathCommand { get; }

        async Task SelectPath(object parameter)
        {
            var dialog = new FolderBrowserDialog { Description = "选择安装位置" };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var path = dialog.SelectedPath.FormatPath().TrimEnd("/");
                if (!path.EndsWith(App.Config.Name)) path = path.CombinePath(App.Config.Name);
                TargetPath = path;

            }
            await Task.CompletedTask;
        }

        public AsyncRelayCommand InstallCommmand { get; }

        async Task Install(object parameter)
        {
            if (!Verify(out var emmebedFiles)) return;
            if (!Directory.Exists(TargetPath)) Directory.CreateDirectory(TargetPath);

            WriteRegistry();
            if (!StopRunningApp()) return;
            RemoveExistFilesByManifest();

            var fileList = new StringBuilder();
            CopyFiles(emmebedFiles, fileList);
            CreateShortcut(fileList);
            File.WriteAllText(FileListPath, fileList.ToString());

            MainViewModel.Index++;
            await Task.CompletedTask;
        }

        bool Verify(out List<string> emmebedFiles)
        {
            emmebedFiles = new List<string>();
            if (string.IsNullOrWhiteSpace(TargetPath))
            {
                MessageBox.Show("请选择安装位置");
                return false;
            }

            emmebedFiles = App.Assembly.GetManifestResourceNames().Where(x => x.StartsWith("Data/Bin/")).Select(x => x.TrimStart("Data/Bin/")).ToList();
            if (emmebedFiles.IsNullOrEmpty())
            {
                MessageBox.Show("二进制文件不能为空");
                return false;
            }

            if (emmebedFiles.Count(x => x == App.Config.ExePath) != 1)
            {
                MessageBox.Show("找不到可执行文件");
                return false;
            }

            return true;
        }

        void WriteRegistry()
        {
            RegistryExtension.Set(TargetPath);
        }

        bool StopRunningApp()
        {
            var processes = Process.GetProcessesByName(App.Config.Name).Where(x => x.MainModule.FileName.FormatPath() == ExePath).ToList();
            if (!processes.Any()) return true;

            if (MessageBox.Show("确定停止正在运行的程序?", "消息确认", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                foreach (var process in processes)
                {
                    process.Kill();
                    process.WaitForExit();
                    process.Dispose();
                }
                return true;
            }
            else return false;
        }

        void RemoveExistFilesByManifest()
        {
            if (File.Exists(FileListPath))
            {
                var lines = File.ReadAllLines(FileListPath);
                foreach (var line in lines)
                {
                    if (line.StartsWith("file:"))
                    {
                        var path = line.TrimStart("file:");
                        if (Path.IsPathRooted(path)) File.Delete(path);
                        TargetPath.CombinePath(path).RemoveFileIfExist();
                    }
                    else if (line.StartsWith("folder:"))
                    {
                        var path = line.TrimStart("folder:");
                        if (Path.IsPathRooted(path)) Directory.Delete(path, true);
                        var localDirectory = TargetPath.CombinePath(path);
                        if (Directory.Exists(localDirectory)) Directory.Delete(localDirectory, true);
                    }
                }
            }
        }

        void CopyFiles(List<string> emmebedFiles, StringBuilder fileList)
        {
            int size = 0;
            foreach (var file in emmebedFiles)
            {
                var targetPath = TargetPath.CombinePath(file);
                new FileInfo(targetPath).DirectoryName.CreateDirectoryIfNotExist();
                fileList.AppendLine($"file:{file}");
                var stream = App.Assembly.GetManifestResourceStream($"Data/Bin/{file}");
                stream.SaveToFile(targetPath);
                size += (int)stream.Length;
            }
            RegistryExtension.SetSystemSizeInformation(size / 1024);
        }

        void CreateShortcut(StringBuilder fileList)
        {
            var path = ShortcutExtension.CreateShortcutToDesktop(TargetPath);
            var name = path.GetFileName();

            //desktop
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory).CombinePath(name);
            fileList.AppendLine($"file:{desktopPath}");
            File.Copy(path, desktopPath);

            //user start
            //user start path->C:\Users\{user}\AppData\Roaming\Microsoft\Windows\Start Menu\Programs
            //system start path->C:\ProgramData\Microsoft\Windows\Start Menu\Programs
            var userStartPath = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu).CombinePath($"Programs/{name}");
            fileList.AppendLine($"file:{userStartPath}");
            File.Copy(path, userStartPath);

            path.RemoveFileIfExist();
        }
    }
}