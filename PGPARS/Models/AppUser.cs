using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace PGPARS.Models
{
    public class AppUser : IdentityUser
    {

       
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public string Position { get; set; }
        public string Nnumber { get; set; }
        
        
    }
}
