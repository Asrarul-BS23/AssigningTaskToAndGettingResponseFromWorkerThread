using System;
using System.Threading.Tasks;

namespace AsyncAwaitDemo
{
    class Program
    {
        // An async method that “does work” for the given duration,
        // then returns a string result.
        static async Task<string> DoWorkAsync(TimeSpan duration)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Worker: Starting {duration.TotalSeconds}s task...");

            // This yields control back to the caller and doesn't block
            await Task.Delay(duration);

            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Worker: Task complete.");
            return $"Result after {duration.TotalSeconds}s";
        }

        // C# 7.1+ allows async Main
        static async Task Main(string[] args)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Main: Kick off async work for 1 minute");
            Task<string> workTask = DoWorkAsync(TimeSpan.FromMinutes(1));

            // Meanwhile, do other work in the main thread WITHOUT blocking it
            for (int i = 1; i <= 12; i++)
            {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Main: Doing other work... ({i * 5}s elapsed)");
                await Task.Delay(TimeSpan.FromSeconds(5));
            }

            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Main: Other work done—now awaiting worker result");

            // This await will complete immediately if DoWorkAsync already finished,
            // or else it will asynchronously resume when the worker completes.
            string result = await workTask;

            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Main: Got result -> {result}");
            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }
    }
}
