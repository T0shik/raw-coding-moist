using Microsoft.AspNetCore.Mvc;

namespace Moist.Api.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Ok();
        }
    }
}