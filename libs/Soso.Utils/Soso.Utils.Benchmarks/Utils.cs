namespace Soso.Utils.Benchmarks
{
    public static class Utils
    {
        public static IEnumerable<T> AsEnumerable<T>(IEnumerable<T> source)
        {
            foreach (var v in source)
            {
                yield return v;
            }
        }
    }
}