using System;
using System.Windows.Input;

namespace StopWatchApp
{
	public abstract class Tab : ViewModelBase, ITab
	{
		public string Name { get; set; }
		public event EventHandler CloseRequested;
		public ICommand CloseCommand { get; }
		public bool CloseVisibility { get; set; }

		protected Tab()
		{
			CloseCommand = new ActionCommand(p => CloseRequested?.Invoke(this, EventArgs.Empty), p => CanClose());
		}

		public virtual bool CanClose()
		{
			return true;
		}
	}
}
