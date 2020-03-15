using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Moist.Configuration.UI.Pages
{
    public abstract class BasePage : PageModel
    {
        protected string UserId =>
            HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                       .Select(c => c.Value)
                       .FirstOrDefault();
    }
}