namespace MakeExeInstaller.Mvvm
{
    public interface IViewModelUserControl<TViewModel> where TViewModel : PageViewModelBase
    {
        TViewModel ViewModel { get; set; }
    }
}
