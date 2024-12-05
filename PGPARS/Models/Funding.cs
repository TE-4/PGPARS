using System;
using System.ComponentModel.DataAnnotations;

namespace PGPARS.Models
{
    public class Funding
    {
        [Key]
        public int FundingID { get; set; } // Unique identifier for funding
        [Required]
        public string? Name { get; set; } // Name of corporation or funding source

      
        public string? Cohort { get; set; } // Cohort for which funding is allocated

      
        public string? FundType { get; set; } // Type of funding (e.g., grant, scholarship)

       
        public string? Source { get; set; } // Source of the funds

        public double? Stipends { get; set; } // Amount allocated for stipends
        public double? Scholarships { get; set; } // Amount allocated for scholarships

        public double? Amount { get; set; } // Total fundable amount

      
        public string? Applicant { get; set; } // Full name of the associated applicant

        public int? ApplicantId { get; set; } // Nullable ID for the associated applicant

        public DateTime DateAdded { get; set; } = DateTime.UtcNow; // Timestamp when the funding was added
        public DateTime DateModified { get; set; } = DateTime.UtcNow; // Timestamp when the funding was last modified

      
        public string? Comment { get; set; } // Additional notes or comments
    }
}
