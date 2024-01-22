using Microsoft.AspNetCore.Identity;

namespace Lapis_Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
