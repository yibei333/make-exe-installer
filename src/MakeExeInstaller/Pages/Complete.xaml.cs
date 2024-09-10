using MakeExeInstaller.Mvvm;
using MakeExeInstaller.ViewModels;
using System.Windows.Controls;

namespace MakeExeInstaller.Pages
{
    /// <summary>
    /// Interaction logic for Disclaimer.xaml
    /// </summary>
    public partial class Complete : UserControl, IViewModelUserControl<CompleteViewModel>
    {
        public Complete()
        {
            InitializeComponent();
        }

        public CompleteViewModel ViewModel { get; set; }
    }
}
