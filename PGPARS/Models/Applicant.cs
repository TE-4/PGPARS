using System;

namespace PGPARS.Models
{
    public class Applicant
    {
        public int Id { get; set; }
        public string? Nnumber { get; set; }
        public  string? FirstName { get; set; }
        public  string? LastName { get; set; }
        public DateTime? AppSubmitDate { get; set; }
        public  string? Email { get; set; }
        public  string? Phone { get; set; }
        public  string? Status { get; set; }
        public  string? Race { get; set; }
        public  string? Sex { get; set; }
        public  string? Reviewer1 { get; set; }
        public  string? Reviewer2 { get; set; }
        public  string? Rev1Decision { get; set; }
        public  string? Rev2Decision { get; set; }
        public bool? RevAgree { get; set; }
        public  string? CommitteeReview { get; set; }
        public  string? Mentor1 { get; set; }
        public  string? Mentor2 { get; set; }
        public  string? Mentor3 { get; set; }
        public  string? SelectMentor { get; set; }
        public decimal? GPAOverall { get; set; }   // Nullable fields
        public decimal? GPAPsych { get; set; }     // Nullable fields
        public string? GPAComment { get; set; }
        public bool? Course_Req_Met { get; set; }
        public string? CrsReqComment { get; set; }
        public int? LetterQuality { get; set; }
        public string? LetterComment { get; set; }
        public int? ResumeQuality { get; set; }
        public int? ResExpQuality { get; set; }
        public string? ResumeComment { get; set; }
        public int? WritSampQuality { get; set; }
        public  string? WritSampComment { get; set; }
        public int? LORRelevance { get; set; }
        public int? LORQuality { get; set; }
        public  string? LORComment { get; set; }
        public int? OverallFitQuality { get; set; }
        public  string? OverallFitComments { get; set; }
        public  string? DecRec { get; set; }
        public  string? FollowUp { get; set; }
        public  string? FinalComments { get; set; }
    }
}
