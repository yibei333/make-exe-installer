using MakeExeInstaller.ViewModels;
using System.Windows;

namespace MakeExeInstaller
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();
        public MainWindow()
        {
            InitializeComponent();
            TaskbarItemInfo = new System.Windows.Shell.TaskbarItemInfo
            {
                Description="abc",
            };
        }

        private void WindowClose(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
