using Soso.Utils.Random;

namespace Soso.Utils.Benchmarks;

public static class RandomBenchmarks
{
    public static void Run(int iterations)
    {
        int range = 10;
        SosoRandom random = new();
        int[] hits = new int[range];

        for (int i = 0; i < iterations; i++)
        {
            int result = random.Next(0, range);
            hits[result]++;
        }

        for (int i = 0; i < hits.Length; i++)
        {
            Console.WriteLine($"Hit: {i}: {hits[i]}");
        }
    }
}