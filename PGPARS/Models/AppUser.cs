using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace PGPARS.Models
{
    public class AppUser : IdentityUser
    {

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public string Position { get; set; }
        public string Nnumber { get; set; }
        
        
    }
}
