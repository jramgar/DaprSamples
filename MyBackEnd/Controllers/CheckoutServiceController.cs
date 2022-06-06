using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutServiceController : Controller
    {
        [HttpPost("/bindings-twitter")]
        public ActionResult<string> getCheckout([FromBody] int orderId)
        {
            Console.WriteLine("Received Message: " + orderId);
            return "CID" + orderId;
        }
    }
}
