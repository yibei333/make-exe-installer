using MakeExeInstaller.ViewModels;

namespace MakeExeInstaller.Mvvm
{
    public abstract class PageViewModelBase : ViewModelBase
    {
        protected PageViewModelBase(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
        }

        public MainViewModel MainViewModel { get; }
    }
}
