using MakeExeInstaller.Mvvm;
using MakeExeInstaller.ViewModels;
using System.Windows.Controls;

namespace MakeExeInstaller.Pages
{
    /// <summary>
    /// Interaction logic for Disclaimer.xaml
    /// </summary>
    public partial class Disclaimer : UserControl, IViewModelUserControl<DisclaimerViewModel>
    {
        public Disclaimer()
        {
            InitializeComponent();
        }

        public DisclaimerViewModel ViewModel { get; set; }
    }
}
