using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MakeExeInstaller.Mvvm
{
    public class AsyncRelayCommand : ViewModelBase, ICommand
    {
        public event EventHandler CanExecuteChanged;
        bool _isRunning;

        public AsyncRelayCommand(Action<object> onExecute)
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

        Action<object> OnExecute { get; }

        public bool CanExecute(object parameter)
        {
            return !IsRunning;
        }

        public void Execute(object parameter)
        {
            IsRunning = true;
            Task.Run(() =>
            {
                OnExecute(parameter);
            }).ContinueWith((_) => IsRunning = false);
        }
    }
}
