using Microsoft.AspNetCore.Identity;
using System;

namespace PGPARS.Models
{
    public class AppUser : IdentityUser
    {
        public string Nnumber { get; set; }
        // can add more properties later as we need
        // Many properties are built into the IdentityUser (including user roles, email, name, etc.)
    }
}
