using Microsoft.AspNetCore.Mvc;

namespace Moist.Application.Api.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Ok();
        }
    }
}