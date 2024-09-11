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

        public AsyncRelayCommand SelectPathCommand { get; }

        async Task SelectPath(object parameter)
        {
            var dialog = new FolderBrowserDialog { Description = "选择安装位置" };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                TargetPath = dialog.SelectedPath;
            }
            await Task.CompletedTask;
        }

        public AsyncRelayCommand InstallCommmand { get; }

        async Task Install(object parameter)
        {
            if (!Verify(out var emmebedFiles)) return;
            var directory = Path.Combine(TargetPath, App.Config.Name);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            WriteRegistry();
            StopRunningApp(directory);
            RemoveExistFilesByManifest(directory);

            var fileListPath = directory.CombinePath("app.file.list");
            var fileList = new StringBuilder();
            CopyFiles(directory, emmebedFiles, fileList);
            CreateShortcut(directory, fileList);
            File.WriteAllText(fileListPath, fileList.ToString());

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

            emmebedFiles = App.Assembly.GetManifestResourceNames().Where(x => x.StartsWith("Data/Bin/")).Select(x=>x.TrimStart("Data/Bin/")).ToList();
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

        void StopRunningApp(string directory)
        {
            var exePath = Path.Combine(directory, App.Config.ExePath).FormatPath();
            var processes = Process.GetProcessesByName(App.Config.Name).Where(x => x.StartInfo.FileName.FormatPath() == exePath).ToList();
            if (processes.Any())
            {
                if (MessageBox.Show("确定停止正在运行的程序?", "消息确认", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    processes.ForEach(process =>
                    {
                        process.Kill();
                        process.Dispose();
                    });
                }
            }
        }

        void RemoveExistFilesByManifest(string directory)
        {
            var fileListPath = directory.CombinePath("app.file.list");
            if (File.Exists(fileListPath))
            {
                var lines = File.ReadAllLines(fileListPath);
                foreach (var line in lines)
                {
                    if (line.StartsWith("file:"))
                    {
                        var path = line.TrimStart("file:");
                        if (Path.IsPathRooted(path)) File.Delete(path);
                        directory.CombinePath(path).RemoveFileIfExist();
                    }
                    else if (line.StartsWith("folder:"))
                    {
                        var path = line.TrimStart("folder:");
                        if (Path.IsPathRooted(path)) Directory.Delete(path, true);
                        var localDirectory = directory.CombinePath(path);
                        if (Directory.Exists(localDirectory)) Directory.Delete(localDirectory, true);
                    }
                }
            }
        }

        void CopyFiles(string directory, List<string> emmebedFiles, StringBuilder fileList)
        {

            var binPath = directory.CombinePath("bin");
            binPath.CreateDirectoryIfNotExist();

            foreach (var file in emmebedFiles)
            {
                var targetPath = binPath.CombinePath(file);
                new FileInfo(targetPath).DirectoryName.CreateDirectoryIfNotExist();
                fileList.AppendLine($"file:bin/{file}");
                var stream = App.Assembly.GetManifestResourceStream($"Data/Bin/{file}");
                stream.SaveToFile(targetPath);
            }
        }

        void CreateShortcut(string directory, StringBuilder fileList)
        {
            var path = ShortcutExtension.CreateShortcutToDesktop(directory);
            var name = path.GetFileName();
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory).CombinePath(name);
            fileList.AppendLine($"file:{desktopPath}");
            File.Copy(path, desktopPath);

            //var startMenuDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu).CombinePath($"Programs/{App.Config.Name}");
            //startMenuDirectory.CreateDirectoryIfNotExist();
            //fileList.AppendLine($"folder:{startMenuDirectory}");
            //var startMenuPath = startMenuDirectory.CombinePath(name);
            //File.Copy(path, startMenuPath);

            //var userStartMenuDirectory = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu).CombinePath($"Programs/{App.Config.Name}");
            //userStartMenuDirectory.CreateDirectoryIfNotExist();
            //fileList.AppendLine($"folder:{userStartMenuDirectory}");
            //var userStartMenuPath = userStartMenuDirectory.CombinePath(name);
            //File.Copy(path, userStartMenuPath);

            var userStartPath = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu).CombinePath($"Programs/{name}");
            fileList.AppendLine($"file:{userStartPath}");
            File.Copy(path, userStartPath);

            var startPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu).CombinePath($"Programs/{name}");
            fileList.AppendLine($"file:{startPath}");
            File.Copy(path, startPath);

            //todo:add app to installed list
            //todo:modify taskbar name
        }
    }
}