using Microsoft.AspNetCore.Identity;

namespace JWTlesson1.API.Enums.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Role Role { get; set; }
    }
}
