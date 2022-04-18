using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Scheduler.Tests
{
    public class Tests
    {
        const int AdditionalWaitMs = 200;

        readonly Random Random = new Random();
        TimeScheduler Scheduler;

        [SetUp]
        public void Setup()
        {
            Scheduler = new TimeScheduler();
        }

        [Test]
        public void TestTimerShouldEvaluete()
        {
            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            
            var actual = string.Empty;
            var sw = new Stopwatch();
            sw.Start();

            var timeout = 3000;

            SideTimer timer1 = SideTimerBuilder.Build()
                    .Action(() => { autoResetEvent.Set(); sw.Stop(); actual = "invoked"; })
                    .Timeout(timeout)
                    .AddToScheduler(Scheduler);

            Assert.IsTrue(autoResetEvent.WaitOne());
            Assert.Less(sw.ElapsedMilliseconds, timeout + AdditionalWaitMs);
            Assert.AreEqual("invoked", actual);
        }

        [Test]
        public void TestTimerShouldNotEvaluete()
        {
            AutoResetEvent autoResetEvent = new AutoResetEvent(false);

            var actual = string.Empty;
            var sw = new Stopwatch();
            sw.Start();

            var timeout = 3000;

            SideTimer timer1 = SideTimerBuilder.Build()
                    .Action(() => { autoResetEvent.Set(); sw.Stop(); actual = "invoked"; })
                    .Timeout(timeout)
                    .AddToScheduler(Scheduler);

            Assert.IsFalse(autoResetEvent.WaitOne(2900));
            Assert.Greater(sw.ElapsedMilliseconds, timeout - AdditionalWaitMs);
            Assert.AreEqual(String.Empty, actual);
        }

        [Test]
        public void TestManyTimersShouldEvaluete()
        {
            List<Tuple<AutoResetEvent, Stopwatch, ActualWrap>> list = new();

            for (int i = 0; i < 1000; i++)
            {
                AutoResetEvent autoResetEvent = new AutoResetEvent(false);

                var sw = new Stopwatch();
                sw.Start();
                var timeout = Random.Next(2000, 10000);

                var actual = new ActualWrap(timeout, string.Empty);

                SideTimer timer = SideTimerBuilder.Build()
                    .Action(() => { autoResetEvent.Set(); sw.Stop(); actual.Value = "invoked"; })
                    .Timeout(timeout)
                    .AddToScheduler(Scheduler);

                list.Add(Tuple.Create(autoResetEvent, sw, actual));
            }

            int workerThreads, complete;
            ThreadPool.GetMinThreads(out workerThreads, out complete);

            ThreadPool.SetMinThreads(2000, complete);

            IList<Task> tasks = new List<Task>();
            foreach (var item in list)
            {
                Task task = new(() =>
                {
                    var (autoResetEvent, sw, actual) = item;
                    Assert.IsTrue(autoResetEvent.WaitOne(actual.Timeout + AdditionalWaitMs));
                    Assert.Less(sw.ElapsedMilliseconds, actual.Timeout + AdditionalWaitMs);
                    Task.Delay(1).Wait();
                    Assert.AreEqual("invoked", actual.Value);
                });
                task.Start();
                tasks.Add(task);
            }

            Task.WhenAll(tasks.AsParallel()).Wait();
        }

        [Test]
        public void TestManyTimersShouldNotEvaluete()
        {
            List<Tuple<AutoResetEvent, Stopwatch, ActualWrap>> list = new();

            for (int i = 0; i < 1000; i++)
            {
                AutoResetEvent autoResetEvent = new AutoResetEvent(false);

                var sw = new Stopwatch();
                sw.Start();
                var timeout = Random.Next(5000, 10000);

                var actual = new ActualWrap(timeout, string.Empty);

                SideTimer timer = SideTimerBuilder.Build()
                    .Action(() => { autoResetEvent.Set(); sw.Stop(); actual.Value = "invoked"; })
                    .Timeout(timeout)
                    .AddToScheduler(Scheduler);

                list.Add(Tuple.Create(autoResetEvent, sw, actual));
            }

            int workerThreads, complete;
            ThreadPool.GetMinThreads(out workerThreads, out complete);

            ThreadPool.SetMinThreads(2000, complete);

            IList<Task> tasks = new List<Task>();
            foreach (var item in list)
            {
                Task task = new(() =>
                {
                    var (autoResetEvent, sw, actual) = item;
                    Assert.IsFalse(autoResetEvent.WaitOne(actual.Timeout - AdditionalWaitMs * 5));
                    Assert.AreEqual(string.Empty, actual.Value);
                });
                task.Start();
                tasks.Add(task);
            }

            Task.WhenAll(tasks.AsParallel()).Wait();
        }
    }

    public class ActualWrap
    {
        public int Timeout { get; set; }
        public string Value { get; set; }

        public ActualWrap()
        {
        }

        public ActualWrap(int timeout, string value)
        {
            Timeout = timeout;
            Value = value;
        }
    }

}