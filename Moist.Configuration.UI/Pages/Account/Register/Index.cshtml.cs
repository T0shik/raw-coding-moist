using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;
using Moist.Database;

namespace Moist.Configuration.UI.Pages.Account.Register
{
    public class Index : PageModel
    {
        private readonly IWebHostEnvironment _env;

        public Index(IWebHostEnvironment env)
        {
            _env = env;
        }

        [BindProperty] public RegisterForm Form { get; set; }

        public void OnGet()
        {
            if (_env.IsDevelopment())
            {
                Form = new RegisterForm
                {
                    Email = "bob@test.com",
                    Password = "password",
                    ConfirmPassword = "password"
                };
            }
        }

        public async Task<IActionResult> OnPostAsync(
            [FromServices] UserManager<ApplicationUser> userManager,
            [FromServices] SignInManager<ApplicationUser> signInManager,
            [FromServices] InitialiseShopContext initShop)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = new ApplicationUser(Form.Email);

            var result = await userManager.CreateAsync(user, Form.Password);

            if (!result.Succeeded)
            {
                //todo add some errors                
                return Page();
            }

            if (!await initShop.Initialise(user.Id))
            {
                //todo add some errors
                return Page();
            }

            //todo create verification code and send it to email.
            if (_env.IsDevelopment())
            {
                await signInManager.SignInAsync(user, true);
            }

            return RedirectToPage("/Panel/StoreProfile/Index", new {m = 1});
        }
    }
}