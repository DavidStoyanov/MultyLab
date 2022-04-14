using Scheduler;

namespace ConsoleApp
{
    public class SchedulerDemo
    {
        TimeScheduler Scheduler = new TimeScheduler();
        Random Random = new Random();
        int counter = 0;

        public void TestWithScheduler(int n)
        {
            for (int i = 0; i < n; i++)
            {
                SideTimer timer = SideTimerBuilder.Build()
                    .Action(() => { PrintDateTimeNow(); })
                    .Timeout(Random.Next(0, 10000))
                    .ToTimer();

                Scheduler.Add(timer);
            }
        }

        public void TestOnlyTimers(int n)
        {
            for (int i = 0; i < n; i++)
            {
                var timer = new System.Timers.Timer(Random.Next(1, 10000));
                timer.Elapsed += delegate
                {
                    PrintDateTimeNow();
                    timer.Stop();
                };
                timer.AutoReset = false;
                timer.Start();
            }
        }

        public void TestWithSchedulerTausendTimers(int n)
        {
            Console.WriteLine($"{DateTime.Now}");
            Console.WriteLine("===============");

            SideTimer timer2 = SideTimerBuilder.Build()
                    .Action(PrintDateTimeNow)
                    .Timeout(10000)
                    .AddToScheduler(Scheduler);

            for (int i = 0; i < n; i++)
            {
                SideTimer timer = SideTimerBuilder.Build()
                    .Action(() => PrintDateTimeNow())
                    .Timeout(Random.Next(3000, 5000))
                    .ToTimer();

                Scheduler.Add(timer);

                /*SchedulerTimer timer = new SchedulerTimer()
                {
                    Action = () => PrintDateTimeNow(),
                    End = DateTime.Now
                };*/
            }

            SideTimer timer3 = SideTimerBuilder.Build()
                    .Action(PrintDateTimeNow)
                    .Timeout(20000)
                    .ToTimer();

            Scheduler.Add(timer3);

            SideTimer timer4 = SideTimerBuilder.Build()
                    .Action(PrintDateTimeNow)
                    .Timeout(15000)
                    .ToTimer();

            Scheduler.Add(timer4);
        }

        private void PrintDateTimeNow()
        {
            Console.WriteLine($"{DateTime.Now} - {counter++}");
            Console.WriteLine("===============");
        }
    }
}
