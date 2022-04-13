using Collections;
using Scheduler;

namespace SchedulerResource
{
    public class SchedulerTimers : IDisposable
    {
        private static readonly Comparer<PrivateTimer> DescendingComparer =
            Comparer<PrivateTimer>.Create((x, y) => y.CompareTo(x));

        private readonly Dictionary<SchedulerTimer, PrivateTimer> _timers = new();
        private readonly SortedList<PrivateTimer> _sorted = new SortedList<PrivateTimer>(DescendingComparer);

        public bool Running { get; private set; }

        public SchedulerTimers()
        {
            Running = true;
            Start();
        }

        private void Start()
        {
            Task.Run(() =>
            {
                while (Running == true)
                {
                    if (!_sorted.Any())
                    {
                        // todo: better checks
                        Task.Delay(1000);
                        continue;
                    }

                    double diff =  _sorted.PeekLast().End.Subtract(DateTime.Now).TotalMilliseconds;
                    if (diff > 0)
                    {
                        Task.Delay((int)diff);
                    }
                    else
                    {
                        // todo: remove from _timers
                        PrivateTimer timer = _sorted.RemoveLast();
                        timer.Action();
                    }
                }
            });
        }

        public bool Add(SchedulerTimer timer)
        {
            if (_timers.ContainsKey(timer))
                return false;

            if (DateTime.Now > timer.EndDateTime)
            {
                timer.Action();
                return false;
            }

            PrivateTimer privateTimer = new(timer.Action, timer.EndDateTime);
            
            _timers.Add(timer, privateTimer);
            _sorted.Add(privateTimer);

            return true;
        }

        public bool Remove(SchedulerTimer timer)
        {
            bool isTimerPresent = _timers
                .TryGetValue(timer, out PrivateTimer privateTimer);

            if (!isTimerPresent)
                return false;

            _timers.Remove(timer);

            return _sorted.Remove(privateTimer);
        }

        public void Dispose()
        {
            Running = false;
        }

        private class PrivateTimer : IComparable<PrivateTimer>
        {
            public PrivateTimer(Action action, DateTime end)
            {
                Action = action;
                End = end;
            }

            public Action Action { get; init; }
            public DateTime End { get; init; }


            public int CompareTo(PrivateTimer? other)
            {
                if (other == null)
                    throw new InvalidTimerStateException("Can not compare to null");

                return this.End.CompareTo(other.End);
            }
        }


    }

    public class SchedulerTimer
    {
        public Action Action { get; set; }
        public int? Timeout { get; init; }
        public DateTime? End { get; init; }

        public DateTime EndDateTime => CalculateEndDateTime();

        public SchedulerTimer() { }

        public SchedulerTimer(Action action, int timeout)
        {
            Action = action;
            Timeout = timeout;
        }

        public SchedulerTimer(Action action, DateTime endDateTime)
        {
            Action = action;
            End = endDateTime;
        }

        private DateTime CalculateEndDateTime()
        {
            if (End != null)
                return (DateTime)End;

            if (Timeout != null)
                return DateTime.Now.AddMilliseconds((int)Timeout);

            throw new InvalidTimerStateException("Timer fields not set");
        }

    }
}