using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace StopWatchApp
{
	public class MainWindowViewModel: ViewModelBase
	{
		private ObservableCollection<ITab> _tabs;
		public ObservableCollection<ITab> Tabs {
			get
			{
				return _tabs;
			}
			set
			{
				_tabs = value;
				OnPropertyChanged(nameof(Tab));
			}
		}

		private int _selectedIndex;
		public int SelectedIndex
		{
			get
			{
				return _selectedIndex;
			}
			set
			{				
				// Select Add Tab Button
				if (value == Tabs.Count - 1 && Tabs[value] is PlaceholderTabAdding)
				{
					// Replace Placeholder Tab with StopWatch Tab
					Tabs[value] = CreateStopWatchTab();
					// Add Placeholder Tab if can
					CheckAndAddPlaceholderTab();
				}

				_selectedIndex = value;
				OnPropertyChanged(nameof(SelectedIndex));
			}
		}

		// Counter
		private int _tabCount = 0;

		// Const
		private readonly int _maxTabCount = 10;
		private readonly int _minTabCount = 1;

		public MainWindowViewModel()
		{
			_tabs = new ObservableCollection<ITab>();
			_tabs.CollectionChanged += Tabs_CollectionChanged;

			// First tab
			NewStopWatchTab();
			// Add Tab Button 
			NewPlaceholderTab();
		}

		// Method for adding StopWatch Tabs
		private void NewStopWatchTab()
		{
			Tabs.Add(CreateStopWatchTab());
		}
		// Creating StopWatchTab with template header
		StopWatchTab CreateStopWatchTab()
		{
			return new StopWatchTab(string.Format("Секундомер {0} {1}", ++_tabCount, DateTime.Now.ToString("HH:mm:ss")));
		}

		// Method for adding Placeholder Tabs
		private void NewPlaceholderTab()
		{
			Tabs.Add(new PlaceholderTabAdding());
		}

		// Check count of tabs and if last is not Placeholder tab when add new
		private void CheckAndAddPlaceholderTab()
		{
			if (Tabs.Count < _maxTabCount && !(Tabs[Tabs.Count - 1] is PlaceholderTabAdding))
				NewPlaceholderTab();
		}

		
		private void Tabs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			// Subsribe and unsubscribe close method for all new tabs
			ITab tab;
			switch(e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					tab = (ITab)e.NewItems[0];
					tab.CloseRequested += OnTabCloseRequested;
					break;
				case NotifyCollectionChangedAction.Remove:
					tab = (ITab)e.OldItems[0];
					tab.CloseRequested -= OnTabCloseRequested;
					break;
				case NotifyCollectionChangedAction.Replace:
					tab = (ITab)e.OldItems[0];
					tab.CloseRequested -= OnTabCloseRequested;
					tab = (ITab)e.NewItems[0];
					tab.CloseRequested += OnTabCloseRequested;
					break;
			}
		}

		// Method wich is called on closing tab
		private void OnTabCloseRequested(object sender, EventArgs e)
		{
			// Can't remove tabs when minimum is reached
			if (Tabs.Count <= _minTabCount + 1)
				return;

			var index = Tabs.IndexOf((ITab)sender);
			if (SelectedIndex == Tabs.Count - 2) // If currently selected last tab before placeholder
				SelectedIndex--; // Required to not display the placeholder tab view

			Tabs.RemoveAt(index);

			CheckAndAddPlaceholderTab();
		}
	}
}
