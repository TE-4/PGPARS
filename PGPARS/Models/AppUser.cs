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

        // Properties for keeping track of assigned tasks and how long it takes to complete them
        public DateTime? LastAssignedReview { get; set; }
        public DateTime? LastCompletedReview { get; set; }
        public DateTime? LastAssignedInterview { get; set; }
        public DateTime? LastCompletedInterview { get; set; }

        // Navigation property
        public ICollection<Review> Reviews { get; set; }
    }
}
