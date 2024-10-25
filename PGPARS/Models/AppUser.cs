using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace PGPARS.Models
{
    public class AppUser : IdentityUser
    {

        [Required]
        public string Nnumber { get; set; }

        [Required]
        public string FirstName { get; set; }
  
        [Required]
        public string LastName { get; set; }
        public string ShortName => $"{FirstName[0]}. {LastName}";

        public string? Position { get; set; }
        
   

        
    }
}
