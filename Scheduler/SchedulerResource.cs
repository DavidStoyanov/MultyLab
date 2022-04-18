using Collections;

namespace Scheduler
{
    public class TimeScheduler
    {
        private static readonly Comparer<SideTimer> DescendingComparer =
            Comparer<SideTimer>.Create((x, y) => y.CompareTo(x));

        private readonly SortedList<SideTimer> _timers = new SortedList<SideTimer>(DescendingComparer);

        private CancellationTokenSource _waiterSource = new CancellationTokenSource();

        private Task? _process;
        private bool _enabled;

        public bool Enabled
        {
            get => _enabled;
            set => SetEnable(value);
        }


        public TimeScheduler() : this(true) { }

        public TimeScheduler(bool enabled)
        {
            Init(enabled);
        }


        public bool Add(SideTimer timer)
        {
            if (DateTime.Now > timer.End)
            {
                timer.Action();
                return false;
            }

            lock (_timers)
            {
                _timers.Add(timer);
            }

            RestartProcess();

            return true;
        }

        public bool Remove(SideTimer timer)
        {
            lock (_timers)
            {
                return _timers.Remove(timer);
            }
        }

        public IList<SideTimer> RemoveAll()
        {
            return RemoveAll(false);
        }

        public IList<SideTimer> RemoveAll(bool evaluate)
        {
            IList<SideTimer> timers = new List<SideTimer>();

            lock (_timers)
            {
                foreach (var timer in _timers)
                    timers.Add(timer);

                _timers.Clear();
            }

            if (evaluate)
            {
                foreach (var timer in _timers)
                    timer.Action.Invoke();
            }

            return timers;
        }

        private void StartProcessing()
        {
            _process = Task.Run(() =>
            {
                while (_enabled)
                {
                    if (!_timers.Any()) break;

                    int diff = GetDifference();

                    if (diff >= 1)
                    {
                        _waiterSource.Token.WaitHandle.WaitOne(diff);
                        ResetWaiterSourceIfWasRequested();
                    }
                    else
                    {
                        SideTimer timer = RemoveLastTimer();
                        timer.Action.Invoke();
                    }
                }

            });
        }

        private int GetDifference()
        {
            lock (_timers)
            {
                SideTimer timer = _timers.Last();
                return (int)timer.End.Subtract(DateTime.Now).TotalMilliseconds;
            }
        }

        private SideTimer RemoveLastTimer()
        {
            lock (_timers)
            {
                return _timers.RemoveLast();
            }
        }

        private void ResetWaiterSourceIfWasRequested()
        {
            if (!_waiterSource.IsCancellationRequested) return;
            _waiterSource.Dispose();
            _waiterSource = new CancellationTokenSource();
        }

        private void RestartProcess()
        {
            if (!_enabled) return;

            _waiterSource.Cancel();

            if (_process.IsCompleted)
            {
                _process.Dispose();
                StartProcessing();
            }
        }

        private void Init(bool enabled)
        {
            _enabled = enabled;
            _waiterSource = new CancellationTokenSource();

            if (_enabled)
            {
                StartProcessing();
            }
            else
            {
                _process = Task.CompletedTask;
            }
        }

        private void SetEnable(bool value)
        {
            _enabled = value;
            if (_enabled) RestartProcess();
        }


    }


    public class SideTimer : IComparable<SideTimer>
    {
        public Action Action { get; set; }
        public DateTime End { get; init; }

        public SideTimer()
        {
        }

        public SideTimer(Action action, DateTime end)
        {
            Action = action;
            End = end;
        }

        public int CompareTo(SideTimer? other)
        {
            return this.End.CompareTo(other.End);
        }
    }

    public class ASD
    {
        public static void Main()
        {
            TimeScheduler scheduler = new TimeScheduler();
            var x = SideTimerBuilder.Build().Action(() => { }).Timeout(3000).AddToScheduler(scheduler);
        }
    }


    public class SideTimerBuilder
    {
        protected Action? _action;
        protected int? _timeout;
        protected DateTime? _end;

        private SideTimerBuilder() { }

        public static BuilderTimer Build()
        {
            return new BuilderTimer();
        }

        public class BuilderTimer : SideTimerBuilder
        {
            public BuilderAction Action(Action action)
            {
                _action = action;
                return (BuilderAction)this;
            }
        }

        public class BuilderAction : BuilderTimer
        {
            public BuilderTime Timeout(int timeout)
            {
                _timeout = timeout;
                return (BuilderTime)this;
            }

            public BuilderTime EndDateTime(DateTime end)
            {
                _end = end;
                return (BuilderTime)this;
            }
        }

        public class BuilderTime : BuilderAction
        {
            public SideTimer ToTimer()
            {
                return BuildTimer();
            }

            public SideTimer AddToScheduler(TimeScheduler scheduler)
            {
                SideTimer timer = BuildTimer();
                scheduler.Add(timer);
                return timer;
            }
        }

        
        private SideTimer BuildTimer()
        {
            if (_action == null)
                throw new Exception("Timer build exception: missing action");

            return new SideTimer(_action, ExtractEndDateTime());
        }

        private DateTime ExtractEndDateTime()
        {
            if (_end != null)
                return (DateTime)_end;

            if (_timeout != null)
                return DateTime.Now.AddMilliseconds((int)_timeout);

            throw new Exception("Should initiate atleast End or Timeout");
        }
    }

}
