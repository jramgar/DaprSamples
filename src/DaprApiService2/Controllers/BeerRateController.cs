using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

namespace ApiService2.Controllers;

[ApiController]
[Route("[controller]")]
public class BeerRateController : ControllerBase
{
    private static readonly string[] Beers = new[]
    {
        "Mahou", "San Miguel", "Cruzcampo", "Amstel", "Estrella Galicia", "Damm"
    };


    private readonly ILogger<BeerRateController> _logger;
    private readonly DaprClient _daprClient;

    const string storeName = "statestore";

    public BeerRateController(ILogger<BeerRateController> logger, DaprClient daprClient)
    {
        _logger = logger;
        _daprClient = daprClient;
    }

    [HttpGet(Name = "GetBeerRate")]
    public async Task<IEnumerable<BeerRate>> Get(CancellationToken cancellationToken)
    {
        var stateResult = await _daprClient.GetStateEntryAsync<IEnumerable<BeerRate>>(storeName, "beer:*", cancellationToken: cancellationToken);
        if (stateResult != null && stateResult.Value != null)
        {
            return stateResult.Value;
        }
        return Enumerable.Empty<BeerRate>();
    }
    //public IEnumerable<BeerRate> Get()
    //=> Beers.Select(b => new BeerRate
    //{
    //    Name = b,
    //    Rate = Random.Shared.Next(0, 100)
    //}).ToArray();


    [HttpPut(Name = "PutBeerRate")]
    public async Task<IActionResult> Put(BeerRate beerRate, CancellationToken cancellationToken)
    {
        var beer = Beers.SingleOrDefault(b => b == beerRate.Name);
        if (beer == null)
        {
            return NotFound();
        }

        //await QueryStateStore(beerRate, beer, cancellationToken);

        var stateResult = await _daprClient.GetStateEntryAsync<BeerRate>(storeName, beer, cancellationToken: cancellationToken);
        if (stateResult != null && stateResult.Value != null)
        {
            stateResult.Value.SetRate(beerRate.Rate);
            beerRate = stateResult.Value;
            Console.WriteLine($"Recover from state {beerRate.Name} with {beerRate.Rates} rates and value: {beerRate.Rate}");
        }

        await _daprClient.SaveStateAsync<BeerRate>(storeName, $"beer:{beer}", beerRate, cancellationToken: cancellationToken);
        return Ok();
    }

    private async Task<BeerRate> QueryStateStore(BeerRate beerRate, string? beer, CancellationToken cancellationToken)
    {
        var query = "{" +
                "\"filter\": {" +
                    "\"EQ\": { \"value.name\": \"" + beer + "\" }" +
                "}," +
            "}";


        var queryResults = await _daprClient.QueryStateAsync<BeerRate>(storeName, query, cancellationToken: cancellationToken);

        if (queryResults != null && queryResults.Results.FirstOrDefault() != null)
        {
            var currentValue = queryResults.Results.First();

            Console.WriteLine($"Recover from state {currentValue.Data.Name} with {currentValue.Data.Rates} rates and value of {currentValue.Data.Rate}");

            beerRate.Rates = currentValue.Data.Rates++;
            beerRate.Rate = (currentValue.Data.Rate + beerRate.Rate) / 2;
        }

        return beerRate;
    }
}
