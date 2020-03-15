using Microsoft.AspNetCore.Identity;

namespace Moist.Database
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser(string userName)
            : base(userName) { }
    }
}