using SchedulerResource;

SchedulerTimers scheduler = new SchedulerTimers();
Random random = new Random();

int counter = 0;

for (int i = 0; i < 500; i++)
{
    SchedulerTimer timer = new SchedulerTimer()
    {
        Action = () => PrintDateTimeNow(),
        Timeout = random.Next(10000)
    };

    SchedulerTimer timer2 = new SchedulerTimer()
    {
        Action = () => PrintDateTimeNow2(),
        Timeout = random.Next(10000)
    };

    scheduler.Add(timer);
    scheduler.Add(timer2);
}


void PrintDateTimeNow()
{
    Console.WriteLine($"{DateTime.Now} - {++counter}");
    Console.WriteLine("===============");
}

void PrintDateTimeNow2()
{
    Console.WriteLine($"{DateTime.Now} - {++counter}");
    Console.WriteLine("===============");
}

Console.ReadKey();