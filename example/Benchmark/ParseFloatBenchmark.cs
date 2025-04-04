using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark
{
    [SimpleJob(1, 10, 15, 1_000_000)]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.Declared)]
    public class ParseFloatBenchmark
    {
        public IEnumerable<byte[]> Data()
        {
            yield return new byte[] {  (byte)'-', (byte)'4', (byte)'2', (byte)'.', (byte)'2', (byte)'4' };
            //yield return new char[] { '4', '2', '.', '2', '4' };
        }


        [Benchmark(Baseline = true)]
        [ArgumentsSource(nameof(Data))]        
        public float ParseFloat_StdLib(byte[] data)
        {
            return float.Parse(data);
        }


        
        [ArgumentsSource(nameof(Data))]        
        [Benchmark]
        public float ParseFloat(byte[] data)
        {
            ReadOnlySpan<byte> readOnlySpan = data;
            bool isNegative = readOnlySpan[0] == '-';
            var withoutsign = readOnlySpan[(isNegative ? 1 : 0)..];
            int comma = withoutsign.IndexOf((byte)'.');

            float val = 0;

            Span<float> pow = stackalloc float[3] { 1, 10, 100 };

            // before ,
            for (int i = 0; i < comma; i++)
            {
                val += Digit(withoutsign[i]) * pow[comma - i - 1];
            }

            // after ,
            for (int i = comma + 1; i < withoutsign.Length; i++)
            {
                val += Digit(withoutsign[i]) / pow[i - comma];
            }

            if (isNegative)
                return -val;

            return val;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private float Digit(byte v)
        {
            switch (v)
            {
                case (byte)'0': return 0;
                case (byte)'1': return 1;
                case (byte)'2': return 2;
                case (byte)'3': return 3;
                case (byte)'4': return 4;
                case (byte)'5': return 5;
                case (byte)'6': return 6;
                case (byte)'7': return 7;
                case (byte)'8': return 8;
                case (byte)'9': return 8;
                default:
                    throw new InvalidDataException();
            }
        }

        [ArgumentsSource(nameof(Data))]
        [Benchmark]
        public float ParseFloat2(byte[] data)
        {
            ReadOnlySpan<byte> readOnlySpan = (data);
            bool isNegative = readOnlySpan[0] == '-';
            var withoutsign = readOnlySpan[(isNegative ? 1 : 0)..];
            int comma = withoutsign.IndexOf((byte)'.');

            float val = 0;

            Span<float> pow = stackalloc float[3] { 1, 10, 100 };

            // before ,
            for (int i = 0; i < comma; i++)
            {
                val += (withoutsign[i] - '0') * pow[comma - i - 1];
            }

            // after ,
            for (int i = comma + 1; i < withoutsign.Length; i++)
            {
                val += (withoutsign[i] - '0') / pow[i - comma];
            }

            if (isNegative)
                return -val;

            return val;
        }
    }
}
