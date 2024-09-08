using MakeExeInstaller.Mvvm;
using System.Windows;

namespace MakeExeInstaller.ViewModels
{
    public class AgreementViewModel : PageViewModelBase
    {
        public AgreementViewModel(MainViewModel mainViewModel) : base(mainViewModel)
        {
            AgreeCommand = new AsyncRelayCommand(Agree);
            DisAgreeCommand = new AsyncRelayCommand(DisAgree);
        }

        public AsyncRelayCommand AgreeCommand { get; }

        void Agree(object parameter)
        {
            MainViewModel.Index++;
        }

        public AsyncRelayCommand DisAgreeCommand { get; }

        void DisAgree(object parameter)
        {
            Application.Current.Dispatcher.Invoke(() => Application.Current.Shutdown());
        }
    }
}
