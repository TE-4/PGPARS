using System.ComponentModel.DataAnnotations;

namespace PGPARS.Models
{
    public class ApplicantReviewer  // join table for Applicant and AppUser (Committee)
    {
        [Key]
        [Required]
        public int Id { get; set; } // Primary key

        // Foreign keys
        [Required]
        public string Nnumber { get; set; }
        public Applicant Applicant { get; set; }  // Navigation property

        [Required]
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }  // Navigation property


        // Additional attributes can go here if needed (explicitly
        // creating the join table
        // allows adding more attributes if needed
        // in the future)


    }
}
