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
            if (!_process.IsCompleted) return;

            _waiterSource.Cancel();
            _process.Dispose();

            StartProcessing();
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


    public sealed class SideTimerBuilder
    {
        private Action? _action;
        private int? _timeout;
        private DateTime? _end;

        private SideTimerBuilder() { }

        public static SideTimerBuilder Build()
        {
            return new SideTimerBuilder();
        }


        public SideTimerBuilder Action(Action action)
        {
            _action = action;
            return this;
        }

        public SideTimerBuilder Timeout(int timeout)
        {
            _timeout = timeout;
            return this;
        }

        public SideTimerBuilder EndDateTime(DateTime end)
        {
            _end = end;
            return this;
        }

        public SideTimer AddToScheduler(TimeScheduler scheduler)
        {
            SideTimer timer = BuildTimer();
            scheduler.Add(timer);
            return timer;
        }

        public SideTimer ToTimer()
        {
            return BuildTimer();
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
