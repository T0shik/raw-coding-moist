using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moist.Core.Models;

namespace Moist.Application.Api.Controllers
{
    [Route("/shop")]
    public class ShopController : Controller
    {
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