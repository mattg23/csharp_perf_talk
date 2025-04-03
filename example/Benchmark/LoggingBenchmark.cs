using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark
{
    [SimpleJob(1, 10, 15, 1_000_000)]
    [MemoryDiagnoser]
    public class LoggingBenchmark
    {

        private ILogger<LoggingBenchmark> logger = NullLogger<LoggingBenchmark>.Instance;
        private int x;
        private DateTime test;
        private string static_text;

        [GlobalSetup]
        public void Setup()
        {
            x = 5;
            test = DateTime.Now;
            static_text = $"{x} -- {test}";
        }

        [Benchmark(Baseline = true)]
        public void LogStaticText()
        {
            logger.LogWarning(static_text);
        }

        [Benchmark]
        public void LogStringInterp()
        {
            logger.LogWarning($"{x} -- {test}");
        }

        [Benchmark]
        public void LogStringTemplate()
        {
            logger.LogWarning("{x} -- {test}", x, test);
        }

    }
}
