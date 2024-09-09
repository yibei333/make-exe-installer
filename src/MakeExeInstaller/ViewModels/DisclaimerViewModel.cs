using MakeExeInstaller.Mvvm;
using System.Threading.Tasks;
using System.Windows;

namespace MakeExeInstaller.ViewModels
{
    public class DisclaimerViewModel : PageViewModelBase
    {
        public DisclaimerViewModel(MainViewModel mainViewModel) : base(mainViewModel)
        {
            AgreeCommand = new AsyncRelayCommand(Agree);
            DisAgreeCommand = new AsyncRelayCommand(DisAgree);
        }

        public AsyncRelayCommand AgreeCommand { get; }

        async Task Agree(object parameter)
        {
            MainViewModel.Index++;
            await Task.CompletedTask;
        }

        public AsyncRelayCommand DisAgreeCommand { get; }

        async Task DisAgree(object parameter)
        {
            Application.Current.Shutdown();
            await Task.CompletedTask;
        }
    }
}
