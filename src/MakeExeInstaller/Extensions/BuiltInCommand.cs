using System;
using System.Windows.Input;

namespace MakeExeInstaller.Extensions
{
    /// <summary>
    /// simple command implemention
    /// </summary>
    public class BuiltInCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        /// <summary>
        /// create instance of type BuiltInCommand
        /// </summary>
        /// <param name="execute">execute method</param>
        /// <param name="canExecute">can execute methoed</param>
        public BuiltInCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// can execute changed event
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// command can execute method
        /// </summary>
        /// <param name="parameter">command parameter</param>
        /// <returns>bool</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke(parameter) ?? true;
        }

        /// <summary>
        /// command execution method
        /// </summary>
        /// <param name="parameter">command parameter</param>
        public void Execute(object parameter)
        {
            _execute?.Invoke(parameter);
        }
    }
}
