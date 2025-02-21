using System;
using System.ComponentModel.DataAnnotations;

namespace PGPARS.Models
{
    public class Applicant
    {
        [Key]  // Explicitly mark Nnumber as the primary key
        [Required]
        [RegularExpression(@"^[nN]\d{8}$", ErrorMessage = "Nnumber must be in the format 'nXXXXXXXX' or 'NXXXXXXXX'.")]
        public string Nnumber { get; set; } //personal

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? AppSubmitDate { get; set; } //App related
        public string? Cohort { get; set; } //personal
        public string? email { get; set; } //personal
        public string? Phone { get; set; } //personal
        public string? AdvisorEmail { get; set; } //personal
        public string? Status { get; set; } //Most significant, implement slider bar?
        public string? Race { get; set; } //personal
        public string? Sex { get; set; } //personal
        public bool? Rev1Decision { get; set; } //Review / Committee
        public bool? Rev2Decision { get; set; } //Review / Committee
        public bool? RevAgree { get; set; } //Review / Committee
        public string? Mentor1 { get; set; } //mentor / advisor?
        public string? Mentor2 { get; set; } //mentor / advisor?
        public string? Mentor3 { get; set; } //mentor / advisor?
        public string? Mentor4 { get; set; } //mentor / advisor?
        public string? SelectMentor { get; set; } //mentor / advisor?
        //[Required]
        [Range(0.0, 5.0, ErrorMessage = "GPA Overall must be between 0.0 and 5.0.")]
        public decimal? GPAOverall { get; set; } //General Stats
        //[Required]
        [Range(0.0, 5.0, ErrorMessage = "GPA Psychology must be between 0.0 and 5.0.")]
        public decimal? GPAPsych { get; set; } //General Stats
        public string? GPAComment { get; set; } //General Stats
        public bool? Course_Req_Met { get; set; } //General Stats
        public string? CrsReqComment { get; set; } //General Stats
           

        // Navigation properties
        public ICollection<Review> Reviews { get; set; }
        public ICollection<FundingAllocations> FundingAllocations { get; set; } 


        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}".Trim();
            }
        }
    }
}
