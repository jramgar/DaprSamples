using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

namespace MyBackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        //private readonly DaprClient _daprClient;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {

            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public Task<IEnumerable<WeatherForecast>> Get()
        {
            //var a = _daprClient.InvokeMethodAsync<IEnumerable<WeatherForecast>>(HttpMethod.Get,
            //var httpClient = DaprClient.CreateInvokeHttpClient("MyBackEnd2");
            //var a = await httpClient.GetAsync("/weatherforecast");
            return GetForecast(5);
        }

        [Topic("servicebus-pubsub", "newRequest", "event.data.numberOfDays <= 7", 1)]
        [HttpPost("/shortforecast")]
        public async Task<ActionResult> HandleShortForecast(WeatherForecastRequest request)
        {
            Console.WriteLine("Calculating short forecast");
            var forecast = await GetForecast(request.NumberOfDays);
            return Ok(forecast);
        }

        [Topic("servicebus-pubsub", "newRequest", "event.data.numberOfDays > 7", 2)]
        [HttpPost("/longforecast")]
        public async Task<ActionResult> HandleLongForecast(WeatherForecastRequest request)
        {
            Console.WriteLine("Calculating long forecast");
            var forecast = await GetForecast(request.NumberOfDays);
            return Ok(forecast);
        }

        [Topic("servicebus-pubsub", "newRequest")]
        [HttpPost("/defaultforecast")]
        public async Task<ActionResult> HandleDefaultForecast(WeatherForecastRequest request)
        {
            Console.WriteLine("Calculating default forecast");
            var forecast = await GetForecast(request.NumberOfDays);
            return Ok(forecast);
        }

        private async Task<IEnumerable<WeatherForecast>> GetForecast(int numberOfDays)
        {

            return Enumerable.Range(1, numberOfDays).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
                        .ToArray();
        }
    }
}