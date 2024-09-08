using MakeExeInstaller.Mvvm;
using MakeExeInstaller.ViewModels;
using System.Windows.Controls;

namespace MakeExeInstaller.Pages
{
    /// <summary>
    /// Interaction logic for Agreement.xaml
    /// </summary>
    public partial class Agreement : UserControl, IViewModelUserControl<AgreementViewModel>
    {
        public Agreement()
        {
            InitializeComponent();
        }

        public AgreementViewModel ViewModel { get; set; }
    }
}
