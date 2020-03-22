using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moist.Core;

namespace Moist.Configuration.UI.Controllers.Panel
{
    [Route("panel/[controller]/[action]")]
    public class SchemasController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Index([FromServices] IShopStore store)
        {
            return View(await store.GetSchemas(UserId));
        }

        [HttpGet]
        public IActionResult Create() => View(new CreateSchemaContext.Form());


        [HttpPost]
        public async Task<IActionResult> Create(
            CreateSchemaContext.Form form,
            [FromServices] CreateSchemaContext context)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }

            await context.Create(form);

            return RedirectToAction("Index");
        }
    }
}