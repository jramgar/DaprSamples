using Microsoft.AspNetCore.Mvc;

namespace ApiService2.Controllers;

[ApiController]
[Route("[controller]")]
public class BeerController : ControllerBase
{
    private static readonly string[] Beers = new[]
    {
        "Mahou", "San Miguel", "Cruzcampo", "Amstel", "Estrella Galicia", "Damm"
    };


    private readonly ILogger<BeerController> _logger;

    public BeerController(ILogger<BeerController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetBeer")]
    public IEnumerable<BeerRate> Get()
     => Beers.Select(b => new BeerRate
     {
         Name = b,
         Rate = Random.Shared.Next(0, 100)
     }).ToArray();
}
