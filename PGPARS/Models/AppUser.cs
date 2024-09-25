using Microsoft.AspNetCore.Identity;

namespace PGPARS.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }

        // can add more properties later as we need
        // Many properties are built into the IdentityUser (including user roles)
    }
}
