using MakeExeInstaller.Extensions;
using MakeExeInstaller.Mvvm;
using System;
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
            var directory = WriteRegistry();
            if (directory is null) return;

            StopRunningApp(directory);
            RemoveExistFilesByManifest(directory);

            var fileListPath = directory.CombinePath("app.file.list");
            var fileList = new StringBuilder();
            CopyFiles(directory, fileList);
            CreateShortcut(directory, fileList);
            File.WriteAllText(fileListPath, fileList.ToString());

            MainViewModel.Index++;
            await Task.CompletedTask;
        }

        string WriteRegistry()
        {
            if (string.IsNullOrWhiteSpace(TargetPath))
            {
                MessageBox.Show("请选择安装位置");
                return null;
            }

            try
            {
                RegistryExtension.Set(TargetPath);
                var path = Path.Combine(TargetPath, App.Config.Name);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                return path;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
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

        void CopyFiles(string directory, StringBuilder fileList)
        {

            var binPath = directory.CombinePath("bin");
            binPath.CreateDirectoryIfNotExist();

            var emmebedFiles = App.Assembly.GetManifestResourceNames();
            foreach (var file in emmebedFiles)
            {
                if (!file.StartsWith("Data/Bin")) continue;
                var targetPath = binPath.CombinePath(file.TrimStart("Data/Bin/"));
                new FileInfo(targetPath).DirectoryName.CreateDirectoryIfNotExist();
                fileList.AppendLine($"file:bin/{file.TrimStart("Data/Bin/")}");
                var stream = App.Assembly.GetManifestResourceStream(file);
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

            var startMenuDirectory = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu).CombinePath($"Programs/{App.Config.Name}");
            startMenuDirectory.CreateDirectoryIfNotExist();
            fileList.AppendLine($"folder:{startMenuDirectory}");
            var startMenuPath = startMenuDirectory.CombinePath(name);
            File.Copy(path, startMenuPath);
        }
    }
}