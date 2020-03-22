﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moist.Configuration.UI.ViewModels.Panel;
using Moist.Core;
using Moist.Database;

namespace Moist.Configuration.UI.Controllers
{
    [Route("[controller]/[action]")]

    public class PanelController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public PanelController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index(MessageType m, [FromServices] IShopStore store)
        {
            var vm = await store.GetProfile(UserId, shop => new ShopInformation
            {
                Name = shop.Name,
                Description = shop.Description,
            });

            ViewData["Message"] = m.ToMessage();

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Profile([FromServices] IShopStore store)
        {
            var vm = await store.GetProfile(UserId, shop => new ChangeShopProfileContext.Form
            {
                Id = shop.Id,
                Name = shop.Name,
                Description = shop.Description
            });

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(
            ChangeShopProfileContext.Form form,
            [FromServices] ChangeShopProfileContext context)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }

            form.UserId = UserId;
            await context.Change(form);

            return RedirectToAction("Index", "Panel");
        }
    }
}