using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moist.Core;

namespace Moist.Configuration.UI.Pages.Panel.Main
{
    public class Index : BasePage
    {
        public ShopInformation ShopInformation { get; set; }
        
        public async Task OnGet([FromServices] IShopStore store)
        {
            ShopInformation = await store.GetProfile(UserId, shop => new ShopInformation
            {
                Name = shop.Name,
                Description = shop.Description,
            });
        }
    }
}