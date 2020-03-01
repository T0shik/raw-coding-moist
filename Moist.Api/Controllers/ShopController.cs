using Microsoft.AspNetCore.Mvc;
using Moist.Core.Models;

namespace Moist.Api.Controllers
{
    [Route("/shop")]
    public class ShopController : Controller
    {
        [HttpGet("{id}")]
        public Shop GetShop(int id)
        {
            return new Shop {Id = id};
        }
    }
}