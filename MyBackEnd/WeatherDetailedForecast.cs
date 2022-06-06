namespace MyBackEnd
{
    public class WeatherDetailedForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        public int WindSpeed { get;set; }
        public int UVIndex { get; set; }

        public string? Summary { get; set; }
    }
}