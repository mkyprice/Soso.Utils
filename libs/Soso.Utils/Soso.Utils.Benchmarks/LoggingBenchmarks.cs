using Soso.Utils.Logging;
using Soso.Utils.Logging.Loggers;
using System.Diagnostics;

namespace Soso.Utils.Benchmarks
{
	public static class LoggingBenchmarks
	{
		[Flags]
		public enum CHANNELS : long
		{
			Debug = long.MinValue,
			Test = long.MaxValue,
		}
		public static string Run(int runs, int iterations, ILogWriter writer)
		{
			SosoLogger<CHANNELS> log = new SosoLogger<CHANNELS>(writer);
			log.ActiveChannels = CHANNELS.Test;

			long runningTotal = 0;
			Console.WriteLine($"Running {runs} runs tests");
			
			for (int run = 0; run < runs; run++)
			{
				Console.Clear();
				Stopwatch sw = new Stopwatch();
				sw.Start();
				for (int i = 0; i < iterations; i++)
				{
					log.Info(CHANNELS.Debug, "DEBUG {i} DEBUG {sw}", i, sw);
					log.Info(CHANNELS.Test, "TEST {i} TEST {sw}", i, sw);
				}
				sw.Stop();
				runningTotal += sw.ElapsedMilliseconds;
				Console.Clear();
			}
			
			long avg = runningTotal / runs;
			string message = $"({nameof(Run)}) Total time for {runs} runs was: {runningTotal}ms. Avg: {avg}ms";
			Console.WriteLine(message);
			return message;

		}
		public static string RunInterpolated(int runs, int iterations, ILogWriter writer)
		{
			SosoLogger<CHANNELS> log = new SosoLogger<CHANNELS>(writer);
			log.ActiveChannels = CHANNELS.Test;

			long runningTotal = 0;
			Console.WriteLine($"Running {runs} runs tests");
			
			for (int run = 0; run < runs; run++)
			{
				Console.Clear();
				Stopwatch sw = new Stopwatch();
				sw.Start();
				for (int i = 0; i < iterations; i++)
				{
					log.Info(CHANNELS.Debug, $"DEBUG {i} DEBUG {sw}");
					log.Info(CHANNELS.Test, $"TEST {i} TEST {sw}");
				}
				sw.Stop();
				runningTotal += sw.ElapsedMilliseconds;
				Console.Clear();
			}
			
			long avg = runningTotal / runs;
			string message = $"({nameof(RunInterpolated)}) Total time for {runs} runs was: {runningTotal}ms. Avg: {avg}ms";
			Console.WriteLine(message);
			return message;
		}
		public static string RunStdOut(int runs, int iterations)
		{
			long runningTotal = 0;
			Console.WriteLine($"Running {runs} runs tests");
			
			for (int run = 0; run < runs; run++)
			{
				Console.Clear();
				Stopwatch sw = new Stopwatch();
				sw.Start();
				for (int i = 0; i < iterations; i++)
				{
					Console.WriteLine($"TEST {i} TEST {sw}");
				}
				sw.Stop();
				runningTotal += sw.ElapsedMilliseconds;
				Console.Clear();
			}
			
			long avg = runningTotal / runs;
			string message = $"({nameof(RunStdOut)}) Total time for {runs} runs was: {runningTotal}ms. Avg: {avg}ms";
			Console.WriteLine(message);
			return message;
		}
	}
}
