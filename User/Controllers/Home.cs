using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace User.Controllers
{
    [Route("[controller]")]
    public class Home : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [Route("shops")]
        public async Task<IActionResult> Shops()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync("https://localhost:5001/shops");
            var content = await response.Content.ReadAsStringAsync();
            var shops = JsonConvert.DeserializeObject<IEnumerable<ShopVm>>(content);
            return View(shops);
        }
    }

    public class ShopVm
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
}