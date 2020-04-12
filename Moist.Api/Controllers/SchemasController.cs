using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moist.Application.Api.Controllers;
using Moist.Core;

namespace Moist.Configuration.UI.Controllers.Panel
{
    [Authorize]
    [Route("panel/[controller]")]
    public class SchemasController : BaseController
    {

        protected SchemasController(IMediator mediator)
            : base(mediator) { }

        // [HttpGet]
        // public async Task<IActionResult> Index([FromServices] IShopStore store)
        // {
        //     return View(await store.GetSchemas(UserId));
        // }
        //
        // [HttpGet("create")]
        // public IActionResult Create() => View(new CreateSchemaContext.Form());
        //
        // [HttpPost("create")]
        // public async Task<IActionResult> Create(
        //     CreateSchemaContext.Form form,
        //     [FromServices] CreateSchemaContext context)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         return View(form);
        //     }
        //
        //     await context.Create(form);
        //
        //     return RedirectToAction("Index");
        // }
        //
        // [HttpGet("activate")]
        // public IActionResult Activate(
        //     int shopId,
        //     int schemaId,
        //     [FromServices] ActivateSchemaContext context)
        // {
        //     context.Activate(UserId, shopId, schemaId);
        //
        //     return RedirectToAction("Index");
        // }

    }
}