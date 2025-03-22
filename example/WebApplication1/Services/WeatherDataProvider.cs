using Model;

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
    }
}
