using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MakeExeInstaller.Mvvm
{
    public class AsyncRelayCommand : ViewModelBase, ICommand
    {
        public event EventHandler CanExecuteChanged;
        bool _isRunning;

        public AsyncRelayCommand(Func<object, Task> onExecute)
        {
            OnExecute = onExecute;
        }

        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                if (_isRunning == value) return;
                _isRunning = value;
                OnPropertyChanged(nameof(IsRunning));
            }
        }

        Func<object, Task> OnExecute { get; }

        public bool CanExecute(object parameter)
        {
            return !IsRunning;
        }

        public void Execute(object parameter)
        {
            IsRunning = true;
            OnExecute(parameter).ContinueWith((_) =>
            {
                IsRunning = false;
            });
        }

        public async Task ExecuteAsync(object parameter)
        {
            IsRunning = true;
            await OnExecute(parameter);
            IsRunning = false;
        }
    }
}
