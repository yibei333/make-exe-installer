using MakeExeInstaller.Mvvm;
using MakeExeInstaller.ViewModels;
using System.Windows.Controls;

namespace MakeExeInstaller.Pages
{
    /// <summary>
    /// Interaction logic for Install.xaml
    /// </summary>
    public partial class Install : UserControl, IViewModelUserControl<InstallViewModel>
    {
        public Install()
        {
            InitializeComponent();
        }

        public InstallViewModel ViewModel { get; set; }
    }
}
