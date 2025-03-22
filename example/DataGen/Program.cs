using Model;

namespace DataGen
{
    internal class Program
    {
        static readonly string[] top100ProminentCities = new string[]
{
    "New York City", "London", "Paris", "Tokyo", "Singapore", "Beijing", "Los Angeles", "Shanghai", "Hong Kong", "Chicago",
    "Seoul", "Toronto", "Madrid", "San Francisco", "Washington, D.C.", "Brussels", "Melbourne", "Sydney", "Istanbul", "Berlin",
    "Amsterdam", "Barcelona", "Boston", "Dubai", "Miami", "Moscow", "Vienna", "Milan", "Buenos Aires", "Montréal",
    "Bangkok", "Mexico City", "Jakarta", "São Paulo", "Mumbai", "Cairo", "Lagos", "Riyadh", "Kuala Lumpur", "Delhi",
    "Manila", "Bogotá", "Lima", "Santiago", "Johannesburg", "Tel Aviv", "Zurich", "Stockholm", "Copenhagen", "Dublin",
    "Helsinki", "Oslo", "Brisbane", "Perth", "Auckland", "Lisbon", "Warsaw", "Prague", "Budapest", "Athens",
    "Rome", "Munich", "Frankfurt", "Hamburg", "Stuttgart", "Cologne", "Düsseldorf", "Birmingham", "Manchester", "Edinburgh",
    "Glasgow", "Bristol", "Leeds", "Liverpool", "Sheffield", "Newcastle", "Nottingham", "Leicester", "Cardiff", "Belfast",
    "Adelaide", "Gold Coast", "Canberra", "Hobart", "Darwin", "Wellington", "Christchurch", "Queenstown", "Ottawa", "Vancouver",
    "Calgary", "Edmonton", "Quebec City", "Halifax", "Victoria", "Winnipeg", "Toronto", "Montreal", "Ottawa", "Vancouver"
};

        static float[] baseTemps = new float[top100ProminentCities.Length];

        static void Main(string[] args)
        {
            

            var readings_per_city = 650_000;
            long lines = readings_per_city * top100ProminentCities.Length;

            using var file = File.OpenWrite("../WebApplication1/data.csv");
            using var writer = new StreamWriter(file, System.Text.Encoding.UTF8);

            var reading = new TempReading();
            var rnd = new Random(42);

            for (int i = 0; i < top100ProminentCities.Length; i++)
            {
                baseTemps[i] = rnd.Next(-5, 10) / 10.0f;
            }

            void next()
            {
                var i = rnd.Next(0, top100ProminentCities.Length);
                reading.City = top100ProminentCities[i];
                reading.Temp = baseTemps[i] + rnd.Next(-300, 450) / 10.0f;
            }
            ;

            for (int i = 0; i < lines; i++)
            {
                next();
                WriteReading(reading, writer);
            }

        }

        private static void WriteReading(TempReading reading, TextWriter stream)
        {
            stream.Write(reading.City);
            stream.Write(';');
            stream.Write(reading.Temp.ToString("F2", System.Globalization.CultureInfo.InvariantCulture));
            stream.Write(Environment.NewLine);
        }
    }
}
