using System.Collections;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moist.Core;
using Moist.Core.Models;

namespace Moist.Application.Api.Controllers
{
    [Route("/shops")]
    public class ShopController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetShops(
            [FromServices] IShopStore store)
        {
            var shops = await store.GetShops(shop => new
            {
                shop.Id,
                shop.Description,
            });
            return Ok(shops);
        }

        [HttpGet("{id}")]
        public Shop GetShop(int id)
        {
            return new Shop {Id = id};
        }

        [HttpGet("code")]
        public Task<string> ShopCode(
            int storeId,
            int schemaId,
            [FromServices] GenerateRewardCode doer)
        {
            return doer.Create("a", storeId, schemaId);
        }
    }
}