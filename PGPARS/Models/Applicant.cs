using System;
using System.ComponentModel.DataAnnotations;

namespace PGPARS.Models
{
    public class Applicant
    {
        [Key]  // Explicitly mark Nnumber as the primary key
        public string? Nnumber { get; set; } //personal

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? AppSubmitDate { get; set; } //App related
        public string? email { get; set; } //personal
        public string? Phone { get; set; } //personal
        public string? AdvisorEmail { get; set; } //personal
        public string? Status { get; set; } //Most significant, implement slider bar?
        public string? Race { get; set; } //personal
        public string? Sex { get; set; } //personal
        public string? Reviewer1 { get; set; } //Review / Committee
        public string? Reviewer2 { get; set; } //Review / Committee
        public string? Rev1Decision { get; set; } //Review / Committee
        public string? Rev2Decision { get; set; } //Review / Committee
        public bool? RevAgree { get; set; } //Review / Committee
        public string? CommitteeReview { get; set; } //Review / Committee
        public string? Mentor1 { get; set; } //mentor / advisor?
        public string? Mentor2 { get; set; } //mentor / advisor?
        public string? Mentor3 { get; set; } //mentor / advisor?
        public string? SelectMentor { get; set; } //mentor / advisor?
        public decimal? GPAOverall { get; set; } //General Stats
        public decimal? GPAPsych { get; set; } //General Stats
        public string? GPAComment { get; set; } //General Stats
        public bool? Course_Req_Met { get; set; } //General Stats
        public string? CrsReqComment { get; set; } //General Stats
        public int? LetterQuality { get; set; } //App related
        public string? LetterComment { get; set; } //App related
        public int? ResumeQuality { get; set; } //App related
        public int? ResExpQuality { get; set; } //App related
        public string? ResumeComment { get; set; } //App related
        public int? WritSampQuality { get; set; } //App related
        public string? WritSampComment { get; set; } //App related
        public int? LORRelevance { get; set; } //App related
        public int? LORQuality { get; set; } //App related
        public string? LORComment { get; set; } //App related
        public int? OverallFitQuality { get; set; } //App related
        public string? OverallFitComments { get; set; } //App related
        public string? DecRec { get; set; } //App related
        public string? FollowUp { get; set; } //App related
        public string? FinalComments { get; set; } //App related

         // Navigation property for the related Fundings
        public ICollection<Funding>? Fundings { get; set; }
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}".Trim();
            }
        }
    }
}
