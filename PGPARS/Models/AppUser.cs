using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace PGPARS.Models
{
    public class AppUser : IdentityUser
    {

        public string Nnumber { get; set; }
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        public string ShortName { get; set; }

        public string Position { get; set; }
        
   

        
    }
}
