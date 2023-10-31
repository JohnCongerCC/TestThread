using System;
using System.Diagnostics;
using System.Threading.Tasks;

class Program
{
    static Random random = new Random();
    const int NumberOfCalls = 1000;
    const int NumberOfTests = 5;

    static int HardcodedMethod()
    {
        int result = 4;
        int sleepTime = random.Next(5, 45); // Sleep for a random time between 5-45ms
        System.Threading.Thread.Sleep(sleepTime);
        return result;
    }

    static void Main()
    {
        for (int test = 1; test <= NumberOfTests; test++)
        {
            Stopwatch stopwatch = new Stopwatch();

            // Non-threaded version
            stopwatch.Start();
            for (int i = 0; i < NumberOfCalls; i++)
            {
                HardcodedMethod();
            }
            stopwatch.Stop();

            double nonThreadedTime = stopwatch.Elapsed.TotalSeconds;

            // Threaded version
            stopwatch.Reset();
            Task[] tasks = new Task[NumberOfCalls];
            stopwatch.Start();
            for (int i = 0; i < NumberOfCalls; i++)
            {
                int index = i;
                tasks[i] = Task.Run(() => HardcodedMethod());
            }
            Task.WaitAll(tasks);
            stopwatch.Stop();

            double threadedTime = stopwatch.Elapsed.TotalSeconds;

            Console.WriteLine($"Test {test}");
            Console.WriteLine($"Non-threaded Time: {nonThreadedTime:F2} seconds");
            Console.WriteLine($"Threaded Time: {threadedTime:F2} seconds");
            double improvement = nonThreadedTime / threadedTime;
            Console.WriteLine($"Improvement: {improvement:F2}x faster\n");
        }
    }
}
