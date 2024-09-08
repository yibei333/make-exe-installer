using MakeExeInstaller.Mvvm;
using MakeExeInstaller.Pages;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace MakeExeInstaller.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        static readonly string _namespace = typeof(MainViewModel).Namespace;
        int index = -1;
        UserControl currentPage;

        public MainViewModel()
        {
            Pages = new List<UserControl>
            {
                CreatePage<Agreement,AgreementViewModel>(),
                CreatePage<Dependency,DependencyViewModel>(),
                CreatePage<Disclaimer,DisclaimerViewModel>(),
                CreatePage<Install,InstallViewModel>(),
            };
            Index = 0;
        }

        public List<UserControl> Pages { get; }

        public int Index
        {
            get => index;
            set
            {
                if (index == value) return;
                index = value;
                currentPage = Pages[index];
                OnPropertyChanged(nameof(Index));
                OnPropertyChanged(nameof(CurrentPage));
            }
        }

        public UserControl CurrentPage => currentPage;

        UserControl CreatePage<TPage, TViewModel>() where TPage : UserControl, IViewModelUserControl<TViewModel>, new() where TViewModel : PageViewModelBase
        {
            var viewModel = Activator.CreateInstance(typeof(TViewModel), this) as TViewModel;
            return new TPage
            {
                DataContext = viewModel,
                ViewModel = viewModel
            };
        }
    }
}
