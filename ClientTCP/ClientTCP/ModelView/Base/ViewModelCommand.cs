using System;
using System.Windows.Input;

namespace ClientTCP
{
	public class ViewModelCommand : ICommand
	{
		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		private readonly Action<object> _executeAction;

		public ViewModelCommand(Action<object> executeAction)
		{
			_executeAction = executeAction;
		}

		public bool CanExecute(object parameter) => true;
		public void Execute(object parameter) => _executeAction(parameter);
	}
}
