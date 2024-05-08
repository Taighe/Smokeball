using Project.Models;
using System;
using System.Windows.Input;

namespace Project.ViewModels.Commands
{
    public class DataCommand : ICommand
    {
        private Action<object> _execute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public DataCommand(Action<object> actionToExecute)
        {
            _execute = actionToExecute;
        }

        public bool CanExecute(object parameter)
        {
            SEOModel input = (SEOModel)parameter;
            return input != null && !input.IsRequestingData && input.HasData;
        }

        public void Execute(object parameter) => _execute(parameter);
    }
}
