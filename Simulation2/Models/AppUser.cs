using Microsoft.AspNetCore.Identity;

namespace Simulation2.Models
{
    public class AppUser :IdentityUser
    {
        public string LastName { get; internal set; }
        public string FirstName { get; internal set; }
    }
}
