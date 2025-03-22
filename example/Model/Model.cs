namespace Model
{
    public class WeatherDataSummary
    {
        public string City { get; set; } = string.Empty;
        public double MinTemp { get; set; }
        public double MaxTemp { get; set; }
        public double AvgTemp { get; set; }
    }


    public class TempReading
    {
        public string City { get; set; } = string.Empty;
        public float Temp { get; set; }
    }
}
