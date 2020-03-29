using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Moist.Configuration.UI.Controllers
{
    public class BaseController : Controller
    {
        protected string UserId =>
            HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                       .Select(c => c.Value)
                       .FirstOrDefault();

    }
}