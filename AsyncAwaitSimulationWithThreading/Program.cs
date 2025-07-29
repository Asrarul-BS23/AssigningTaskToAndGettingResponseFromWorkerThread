using System;
using System.Threading;

namespace ThreadCallbackDemo
{
    // A simple worker that raises an event when done
    class Worker
    {
        // Event fired on completion, passing back the result string
        public event Action<string> WorkCompleted;

        public void DoWork(TimeSpan duration)
        {
            var thread = new Thread(() =>
            {
                Thread.CurrentThread.Name = "Worker";
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Worker: Starting {duration.TotalSeconds}s task... in: {Thread.CurrentThread.Name} thread");
                Thread.Sleep(duration);  // simulate long work
                string result = $"Result after {duration.TotalSeconds}s";
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Worker: Task complete, raising WorkCompleted from: {Thread.CurrentThread.Name} thread.");
                WorkCompleted?.Invoke(result);
            })
            {
                IsBackground = true
            };
            thread.Start();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var worker = new Worker();
            Thread.CurrentThread.Name = "Main";
            // Subscribe to the completion event
            worker.WorkCompleted += result =>
            {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Main thread callback: Got '{result}' from: {Thread.CurrentThread.Name} thread");
            };

            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Main: Kicking off 1‑minute work from: {Thread.CurrentThread.Name} thread");
            worker.DoWork(TimeSpan.FromMinutes(1));

            // Simulate doing other work on the main thread, without blocking for 1 minute
            for (int i = 1; i <= 12; i++)
            {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Main: Still working on other things... ({i * 5}s elapsed) from: {Thread.CurrentThread.Name} thread");
                Thread.Sleep(TimeSpan.FromSeconds(5));
            }

            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Main: Done with other work. Waiting a bit more for worker to finish if needed...from: {Thread.CurrentThread.Name} thread");
            // Give the worker callback a chance to fire if it hasn't yet
            Thread.Sleep(TimeSpan.FromSeconds(10));

            Console.WriteLine($"Press ENTER to exit. from: {Thread.CurrentThread.Name} thread");
            Console.ReadLine();
        }
    }
}
