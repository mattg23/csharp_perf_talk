using Model;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace WebApplication1.Services
{
    public class WeatherDataProvider
    {
        public List<WeatherDataSummary> GetSummary()
        {
            return File.ReadAllLines("data.csv")
                .Select(line => ParseTempReading(line))
                .GroupBy(tempReading => tempReading.City, tempReading => tempReading.Temp)
                .Select(group => new WeatherDataSummary
                {
                    City = group.Key,
                    MinTemp = group.Min(),
                    MaxTemp = group.Max(),
                    AvgTemp = group.Average()
                })
                .OrderBy(x => x.City)
                .ToList();
        }

        private TempReading ParseTempReading(string line)
        {
            var splitted = line.Split(';');
            var city = splitted[0];
            var temp = float.Parse(splitted[1], System.Globalization.CultureInfo.InvariantCulture);

            return new TempReading
            {
                City = city,
                Temp = temp
            };
        }
        public struct RunningTempData
        {
            public long count;
            public double min;
            public double max;
            public double sum;

        }


        public List<WeatherDataSummary> GetSummaryFast()
        {
            var fi = new FileInfo("data.csv");
            int maxParallelCount = Environment.ProcessorCount;
            var chunkSizeEst = fi.Length / maxParallelCount;

            using var fs = new FileStream("data.csv", FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);

            var dictionary = new Dictionary<string, RunningTempData>(100);

            var tasks = new List<Task<Dictionary<string, RunningTempData>>>();

            var start = 0;
            Span<byte> buffer = stackalloc byte[1024];
            Span<byte> crlf = stackalloc byte[] { (byte)'\r', (byte)'\n' };
            for (int i = 0; i < maxParallelCount; i++)
            {
                var endEst = (i + 1) * chunkSizeEst;
                fs.Seek(endEst, SeekOrigin.Begin);
                fs.Read(buffer);
                var firstbreak = buffer.IndexOf(crlf) + 2;
                var bytes = new byte[(endEst + firstbreak) - start];
                fs.Seek(start, SeekOrigin.Begin);
                var read = fs.Read(bytes, 0, bytes.Length);

                var t = Task.Run(() =>
                {
                    var res = ProcessBlock(bytes[0..read]);
                    return res;
                });
                tasks.Add(t);

                start = read + start;
            }

            Task.WhenAll(tasks).ConfigureAwait(false).GetAwaiter().GetResult();

            var merged = tasks.Select(x => x.Result)
                 .Aggregate(dictionary, (curr, next) =>
                 {
                     foreach (var (key, val) in next)
                     {
                         if (curr.TryGetValue(key, out var cVal))
                         {
                             cVal.sum += val.sum;
                             cVal.count += val.count;
                             cVal.max = Math.Max(val.max, cVal.max);
                             cVal.min = Math.Min(val.min, cVal.min);
                             curr[key] = cVal;
                         }
                         else
                         {
                             curr[key] = val;
                         }
                     }

                     return curr;
                 }).ToList();

            var res = dictionary.Select(x => new WeatherDataSummary
            {
                City = x.Key,
                AvgTemp = (double)x.Value.sum / x.Value.count,
                MaxTemp = x.Value.max,
                MinTemp = x.Value.min,
            }).OrderBy(x => x.City)
            .ToList();

            return res;
        }

        private Dictionary<string, RunningTempData> ProcessBlock(in ReadOnlySpan<byte> block)
        {
            var result = new Dictionary<string, RunningTempData>(100);

            // we control the environment => Newline = \r\n
            // find next newline
            int lineStart = 0;

            for (int i = 0; i < block.Length; i++)
            {
                if (block[i] == '\r')
                {
                    var lineBytes = block[lineStart..i];
                    ParseLine(lineBytes, ref result);
                    i++; // skip \n
                    i++; // i == first character after line break
                    lineStart = i;
                }
            }

            return result;
        }

        private void ParseLine(in ReadOnlySpan<byte> line, ref Dictionary<string, RunningTempData> data)
        {
            var semicolon = line.IndexOf((byte)';');
            var cityBytes = line[0..semicolon];
            var floatBytes = line[(semicolon + 1)..];

            var city = UTF8Encoding.UTF8.GetString(cityBytes);
            float temp = ParseFloat(floatBytes);
            ref var value = ref CollectionsMarshal.GetValueRefOrAddDefault(data, city, out bool exists);

            value.count += 1;
            value.sum += temp;
            value.max = temp > value.max ? temp : value.max;
            value.min = temp < value.min ? temp : value.min;

        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private float ParseFloat(in ReadOnlySpan<byte> readOnlySpan)
        {
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
