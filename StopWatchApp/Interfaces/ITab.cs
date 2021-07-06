using System;
using System.Windows.Input;

namespace StopWatchApp
{
	public interface ITab
	{
		string Name { get; set; }

		event EventHandler CloseRequested;

		ICommand CloseCommand { get; }
		bool CanClose();
		bool CloseVisibility { get; set; }
	}
}
