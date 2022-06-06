using Dapr.Client;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFrontEnd.Controllers;
using System.Linq;

namespace MyFrontEnd.Pages;

public class IndexModel : PageModel
{
    private readonly DaprClient _daprClient;

    public IndexModel(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    public async Task OnGet()
    {
        var forecasts = await _daprClient.InvokeMethodAsync<IEnumerable<WeatherForecast>>(
            HttpMethod.Get,
            "mybackend",
            "weatherforecast");

        var secrets = await _daprClient.GetSecretAsync("localsecretstore", "secret");
        foreach(var secret in secrets)
         { 
        Console.WriteLine(secret);
        }
        //ViewData["WeatherForecastData"] = Enumerable.Empty<WeatherForecast>();
        ViewData["WeatherForecastData"] = forecasts;
        await RequestDetailedForecast();
        await RequestShortForecast();
        await RequestLongForecast();
    }

    private async Task RequestDetailedForecast()
    {
        var data = new WeatherDetailedForecastRequest()
        {
            NumberOfDays = 15
        };
        await _daprClient.PublishEventAsync("servicebus-pubsub", "newDetailedRequest", data);
        Console.WriteLine("PublishEventAsync");
    }

    private async Task RequestShortForecast()
    {
        var data = new WeatherForecastRequest()
        {
            NumberOfDays = 3
        };
        await _daprClient.PublishEventAsync("servicebus-pubsub", "newRequest", data);
        Console.WriteLine("PublishEventAsync");
    }

        private async Task RequestLongForecast()
    {
        var data = new WeatherForecastRequest()
        {
            NumberOfDays = 10
        };
        await _daprClient.PublishEventAsync("servicebus-pubsub", "newRequest", data);
        Console.WriteLine("PublishEventAsync");
    }
}