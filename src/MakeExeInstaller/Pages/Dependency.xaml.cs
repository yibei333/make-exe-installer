using MakeExeInstaller.Mvvm;
using MakeExeInstaller.ViewModels;
using System.Windows.Controls;

namespace MakeExeInstaller.Pages
{
    /// <summary>
    /// Interaction logic for Dependency.xaml
    /// </summary>
    public partial class Dependency : UserControl, IViewModelUserControl<DependencyViewModel>
    {
        public Dependency()
        {
            InitializeComponent();
        }

        public DependencyViewModel ViewModel { get; set; }
    }
}
