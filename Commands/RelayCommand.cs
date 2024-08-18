using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JobTracker.Commands
{
    public class RelayCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public Action <object> _Execute {get; set;}
        public Predicate<object> _CanExecute { get; set; }

        public RelayCommand(Action<object> ExecuteMethod, Predicate<object> CanExecuteMethod)
        {
            _Execute = ExecuteMethod;
            _CanExecute = CanExecuteMethod;
        }

        bool ICommand.CanExecute(object? parameter)
        {
            return _CanExecute(parameter);
        }

        void ICommand.Execute(object? parameter)
        {
            _Execute(parameter);
        }
    }
}
