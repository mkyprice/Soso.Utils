using System.Diagnostics;

namespace Soso.Utils.Benchmarks
{
    public static class IEnumerableBenchmarks
    {
        
        public static void RunToArrayBenchmarks(int count)
        {
            List<int> utilsTests = new List<int>();
            for (int i = 0; i < count; i++)
            {
                utilsTests.Add(System.Random.Shared.Next());
            }
            int[] testArray = utilsTests.ToArray();

            long utilsTime = 0;
            long linqTime = 0;
            int runs = 100;
        
            Console.WriteLine($"Running {runs} runs for {testArray.Length} tests");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < runs; i++)
            {
                int[] utilsArray = Utils.AsEnumerable(testArray).ToArray();
            }
            sw.Stop();
            utilsTime += sw.ElapsedMilliseconds;
            Console.WriteLine($"(Soso.Utils) Total time for {utilsTests.Count} items was: {utilsTime} ms. Avg: {utilsTime / runs} ms over {runs} runs");
        
            sw.Restart();
            for (int i = 0; i < runs; i++)
            {
                int[] linqArray = System.Linq.Enumerable.ToArray(Utils.AsEnumerable(testArray));
            }
            sw.Stop();
            linqTime += sw.ElapsedMilliseconds;
            Console.WriteLine($"(System.Linq) Total time for {utilsTests.Count} items was: {linqTime} ms. Avg: {linqTime / runs} ms over {runs} runs");
        }
    }
}