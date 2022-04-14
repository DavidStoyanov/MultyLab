using System;

namespace ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            var scheduler = new SchedulerDemo();
            scheduler.TestWithSchedulerTausendTimers(10);

            Console.ReadKey();
        }
    }
}