using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Magaz_Stroitelya.VMTools
{
    public class CommandMvvm : ICommand
    {
        Action action;
        Func<bool> canExecute;

        public CommandMvvm(Action action, Func<bool> canExecute)
        {
            this.action = action;
            this.canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
        {
            return canExecute();
        }

        public void Execute(object? parameter)
        {
            action();
        }
    }
    public class ActionCommandMvvm : ICommand
    {
        private readonly Func<Task> action;
        private readonly Func<bool>? canExecute;
        private bool _isExecuting;

        public ActionCommandMvvm(Func<Task> action, Func<bool>? canExecute)
        {
            this.action = action;
            this.canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
        {
            return canExecute();
        }

        public void Execute(object? parameter)
        {
            action();
        }
    }
}
