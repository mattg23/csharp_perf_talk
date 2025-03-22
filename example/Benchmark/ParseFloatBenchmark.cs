using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark
{
    [SimpleJob(1, 10, 15, 1_000_000)]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.Declared)]
    public class ParseFloatBenchmark
    {
        public IEnumerable<char[]> Data()
        {
            yield return new char[] { '-', '4', '2', '.', '2', '4' };
            //yield return new char[] { '4', '2', '.', '2', '4' };
        }


        [Benchmark(Baseline = true)]
        [ArgumentsSource(nameof(Data))]        
        public float ParseFloat_StdLib(char[] data)
        {
            return float.Parse(data);
        }
    }
}
