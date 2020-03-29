using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Store.Controllers
{
    public class Home : Controller
    {
        private readonly HttpClient _http;

        public Home(IHttpClientFactory factory)
        {
            _http = factory.CreateClient();
        }
    
        public async Task<IActionResult> Index(
            int storeId,
            int schemaId)
        {
            var response = await _http.GetAsync($"https://localhost:5001/shop/code?storeId={storeId}&schemaId={schemaId}");
            var code = await response.Content.ReadAsStringAsync();
            return View("Index", code);
        }
    }
}