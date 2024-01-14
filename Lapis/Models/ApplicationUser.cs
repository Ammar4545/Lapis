using Microsoft.AspNetCore.Identity;

namespace Lapis.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string UserName { get; set; }
    }
}
