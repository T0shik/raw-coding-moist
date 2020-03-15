using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moist.Core;

namespace Moist.Configuration.UI.Pages.Panel.StoreProfile
{
    public class Index : BasePage
    {
        public string Message { get; set; }

        [BindProperty]
        public ChangeShopProfileContext.Form Form { get; set; }

        public async Task OnGet(
            [FromServices] IShopStore shopStore,
            MessageType m = MessageType.Nothing)
        {
            SetMessage(m);
            
            Form = await shopStore.GetProfile(UserId, shop => new ChangeShopProfileContext.Form
            {
                Id = shop.Id,
                Name = shop.Name,
                Description = shop.Description
            });
        }

        public async Task<IActionResult> OnPostAsync(
            [FromServices] ChangeShopProfileContext profileContext)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Form.UserId = UserId;
            await profileContext.Change(Form);

            return RedirectToPage("/panel/main/index");
        }
        
        private void SetMessage(MessageType m)
        {
            switch (m)
            {
                case MessageType.Welcome:
                    Message = "Welcome, setup your shop";
                    break;
            }
        }
    }
}