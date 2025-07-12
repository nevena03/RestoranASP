using Microsoft.AspNetCore.Identity;

namespace RestoranASP.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Ime { get; set; }
    }
}
