using System;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Threading;

namespace StopWatchApp
{
	public class StopWatchTab: Tab
	{
        public ICommand StartCommand { get; }
        public ICommand PauseCommand { get; }
        public ICommand ResetCommand { get; }

        private bool _isStop = true;
        public bool IsStop
        {
            get
            {
                return _isStop;
            }
            set
            {
                _isStop = value;
                OnPropertyChanged(nameof(IsStop));
            }
        }

        private bool _isPaused;
        public bool IsPaused
        {
            get
            {
                return _isPaused;
            }
            set
            {
                _isPaused = value;
                OnPropertyChanged(nameof(IsPaused));
            }
        }

        private string _currentTime;
        public string CurrentTime
        {
            get
            {
                return _currentTime;
            }
            set
            {
                if (_currentTime == value)
                    return;
                _currentTime = value;
                OnPropertyChanged(nameof(CurrentTime));
            }
        }

        private string _pauseContinueLabel;
        public string PauseContinueLabel
        {
            get
            {
                return _pauseContinueLabel;
            }
            set
            {
                _pauseContinueLabel = value;
                OnPropertyChanged(nameof(PauseContinueLabel));
            }
        }

        private readonly DispatcherTimer _timer;
        private readonly Stopwatch _stopWatch;

        public StopWatchTab(string name)
		{
            // Set Tab properties
			Name = name;
			CloseVisibility = true;

            // Set Button Command
            StartCommand = new ActionCommand(p => StartStopWatch());
            PauseCommand = new ActionCommand(p => PauseUnpauseStopWatch());
            ResetCommand = new ActionCommand(p => ResetStopWatch());

            // Create and set timers
            _stopWatch = new Stopwatch();
            _timer = new DispatcherTimer(DispatcherPriority.Render)
            {
                Interval = TimeSpan.FromMilliseconds(1)
            };
            _timer.Tick += new EventHandler(UpdateTime);

            // Reset text values
            SetCurrentTime(0, 0, 0);
            PauseContinueLabel = "Пауза";
        }

        // Tab can close only when IsStop is true
        public override bool CanClose()
        {
            return IsStop;
        }

        // Setting text label based on time arguments
        void SetCurrentTime(int minutes, int seconds, int miliseconds)
		{
            CurrentTime = string.Format("{0:00}:{1:00},{2:00}", minutes, seconds, miliseconds);
        }

        private void StartStopWatch()
        {
            _stopWatch.Start();
            _timer.Start();
            IsStop = false;
        }

        private void PauseUnpauseStopWatch()
        {
            if (_stopWatch.IsRunning)
            {
                _stopWatch.Stop();
                IsPaused = true;
                PauseContinueLabel = "Продолжить";
            }
            else
            {
                _stopWatch.Start();
                IsPaused = false;
                PauseContinueLabel = "Пауза";
            }
        }

        private void ResetStopWatch()
        {
            _stopWatch.Reset();
            IsStop = true;
            SetCurrentTime(0, 0, 0);
        }

        private void UpdateTime(object sender, EventArgs e)
        {
            if (_stopWatch.IsRunning)
            {
                TimeSpan ts = _stopWatch.Elapsed;
                SetCurrentTime(ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            }
        }
    }
}
