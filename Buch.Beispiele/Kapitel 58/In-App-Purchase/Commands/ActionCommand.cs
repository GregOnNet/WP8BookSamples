using System;
using System.Windows.Input;

namespace InAppPurchase.Commands
{
    public class ActionCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        public ActionCommand(Action<T> executeAction)
        {
            _execute = executeAction;
            _canExecute = null;
        }

        public ActionCommand(Action<T> executeAction, Func<T, bool> canExecuteFunc)
        {
            _execute = executeAction;
            _canExecute = canExecuteFunc;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }

            return _canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
}
