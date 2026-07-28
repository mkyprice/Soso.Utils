using Soso.Utils.Logging.Loggers;
using System.Diagnostics;

namespace Soso.Utils.Benchmarks;

class Program
{
    static void Main(string[] args)
    {
        // IEnumerableBenchmarks.RunToArrayBenchmarks(10_000_000);

        // RandomBenchmarks.Run(100_000);

        string r1 = LoggingBenchmarks.Run(10, 10_000, new FastConsoleLogger());
        // Console.ReadKey();
        // string r2 = LoggingBenchmarks.Run(10, 10_000, new FastConsoleLogger());
        // string r2 = LoggingBenchmarks.RunInterpolated(100, 10_000, new FastConsoleLogger());
        // string r2 = LoggingBenchmarks.RunStdOut(10, 10_000);
        Console.Clear();
        Console.WriteLine(r1);
        // Console.WriteLine(r2);
    }
}