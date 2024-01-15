using Microsoft.AspNetCore.Identity;

namespace Lapis.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
