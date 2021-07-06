using System;
using System.Windows.Input;

namespace StopWatchApp
{
	public class ActionCommand : ICommand
	{
		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		private readonly Action<Object> action;
		private readonly Predicate<Object> predicate;

		public ActionCommand(Action<Object> action) : this(action, null) { }
		
		public ActionCommand(Action<Object> action, Predicate<Object> predicate)
		{
			this.action = action ?? throw new ArgumentNullException(nameof(action), "Action cant be null");
			this.predicate = predicate;
		}


		public bool CanExecute(object parameter)
		{
			if (predicate == null)
				return true;
			return predicate(parameter);
		}

		public void Execute(object parameter)
		{
			action(parameter);
		}

		public void Execute()
		{
			Execute(null);
		}
	}
}