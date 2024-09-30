using Microsoft.AspNetCore.Identity;
using System;

namespace PGPARS.Models
{
    public class AppUser : IdentityUser
    {
        private int id;

        public int GetId()
        {
            return id;
        }

        public void SetId(int value)
        {
            id = value;
        }

        public required string Nnumber { get; set; }
        public required string Email {  get; set; }


        // can add more properties later as we need
        // Many properties are built into the IdentityUser (including user roles)
    }
}
