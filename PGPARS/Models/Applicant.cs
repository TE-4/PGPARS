using System;
using System.ComponentModel.DataAnnotations;

namespace PGPARS.Models
{
    public class Applicant
    {
        [Key]  // Explicitly mark Nnumber as the primary key
        [Required]
        [RegularExpression(@"^[nN]\d{8}$", ErrorMessage = "Nnumber must be in the format 'nXXXXXXXX' or 'NXXXXXXXX'.")]
        public string Nnumber { get; set; } // primary key

        // Personal information
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? Cohort { get; set; } 
        public string? Email { get; set; } 
        public string? Phone { get; set; } 
        public string? AdvisorEmail { get; set; }
        public string? Status { get; set; } //Most significant, implement slider bar?
        public string? Race { get; set; } 
        public string? Sex { get; set; } 
        public string? PrimaryCitizenship { get; set; } 
        public string? CitizenshipStatus { get; set; } 
        public string? SharePointPdfUrl { get; set; }

        // Program related information
        public string? MissingItems { get; set; } 
        public string? Program { get; set; } 
        public DateTime? AppSubmitDate { get; set; } 

        // General Stats
        [Range(0.0, 5.0, ErrorMessage = "GPA Psychology must be between 0.0 and 5.0.")]
        public decimal? GPAPsych { get; set; } //General Stats

        [Range(0.0, 5.0, ErrorMessage = "GPA Overall must be between 0.0 and 5.0.")]
        public decimal? GPAOverall { get; set; } //General Stats

        public string? GPAComment { get; set; } //General Stats
        public bool? Course_Req_Met { get; set; } //General Stats
        public string? CrsReqComment { get; set; } //General Stats

        // Review Related Information 
        public bool? Rev1Decision { get; set; } //Review / Committee
        public bool? Rev2Decision { get; set; } //Review / Committee
        public bool? RevAgree { get; set; } //Review / Committee
        public int? NumberOfReviews { get; set; } = 0; // Keeps track of how many reviews have been assigned 


        // School Information
        public string? School1Institution { get; set; } //School
        public string? School1Major { get; set; } //School
        public decimal? School1GPA { get; set; } // Last 60+ hour GPA (Grad School only)
        public string? School2Institution { get; set; } //School
        public string? School2Major { get; set; } //School
        public decimal? School2GPA { get; set; } // Last 60+ hour GPA (Grad School only)
        public string? School3Institution { get; set; } //School
        public string? School3Major { get; set; } //School
        public decimal? School3GPA { get; set; } // Last 60+ hour GPA (Grad School only)
        public string? School4Institution { get; set; } //School
        public string? School4Major { get; set; } //School
        public decimal? School4GPA { get; set; } // Last 60+ hour GPA (Grad School only)
        public string? School5Institution { get; set; } //School
        public string? School5Major { get; set; } //School
        public decimal? School5GPA { get; set; } //Last 60+ hour GPA (Grad School only)


        // Mentors 
        public string? Mentor1 { get; set; } //mentor / advisor?
        public string? Mentor2 { get; set; } //mentor / advisor?
        public string? Mentor3 { get; set; } //mentor / advisor?
        public string? Mentor4 { get; set; } //mentor / advisor?
        public string? SelectMentor { get; set; } //mentor / advisor?
        
        
        // Navigation properties
        public ICollection<Review> Reviews { get; set; }
        public ICollection<FundingAllocation> FundingAllocations { get; set; } 


        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}".Trim();
            }
        }
    }
}
