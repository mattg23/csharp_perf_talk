namespace Benchmark
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BenchmarkDotNet.Running.BenchmarkRunner.Run(new Type[]
            {
                typeof(ParseFloatBenchmark),
               // typeof(LoggingBenchmark)
            });
        }
    }
}
