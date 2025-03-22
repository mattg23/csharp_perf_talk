namespace Benchmark
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BenchmarkDotNet.Running.BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }
}
