using BenchmarkDotNet.Attributes;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark
{
    [SimpleJob(1, 10, 15, 1_000_000)]
    [MemoryDiagnoser]
    public class SlideExample
    {
        private byte[] bytes;

        [GlobalSetup]
        public void Setup()
        {
            var rnd = new Random(42);
            bytes = new byte[8192];
            rnd.NextBytes(bytes);
        }

        [Benchmark]
        public double Linq()
        {
            return BitConverter.ToDouble(bytes.Skip(456).Take(8).Reverse().ToArray(), 0);                
        }

        [Benchmark]
        public double Span()
        {
            ReadOnlySpan<byte> spanBytes = bytes[456..(456 + 8)];
            return BinaryPrimitives.ReadDoubleBigEndian(spanBytes);
        }
    }
}
