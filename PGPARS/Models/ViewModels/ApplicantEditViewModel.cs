using System.ComponentModel.DataAnnotations;

namespace PGPARS.Models.ViewModels
{
    public class ApplicantEditViewModel
    {
        [Required]
        [Display(Name = "Nnumber")]
        public string Nnumber { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Display(Name = "Application Submission Date")]
        public DateTime? AppSubmitDate { get; set; }

        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Display(Name = "Phone Number")]
        public string? Phone { get; set; }

        [Display(Name = "Advisor Email")]
        public string? AdvisorEmail { get; set; }

        [Display(Name = "Status")]
        public string? Status { get; set; }

        [Display(Name = "Race")]
        public string? Race { get; set; }

        [Display(Name = "Sex")]
        public string? Sex { get; set; }

        [Display(Name = "Reviewer 1")]
        public string? Reviewer1 { get; set; }

        [Display(Name = "Reviewer 2")]
        public string? Reviewer2 { get; set; }

        [Display(Name = "Reviewer 1 Decision")]
        public bool? Rev1Decision { get; set; }

        [Display(Name = "Reviewer 2 Decision")]
        public bool? Rev2Decision { get; set; }

        [Display(Name = "Reviewers Agree")]
        public bool? RevAgree { get; set; }

        [Display(Name = "Committee Review")]
        public string? CommitteeReview { get; set; }

        [Display(Name = "Mentor 1")]
        public string? Mentor1 { get; set; }

        [Display(Name = "Mentor 2")]
        public string? Mentor2 { get; set; }

        [Display(Name = "Mentor 3")]
        public string? Mentor3 { get; set; }

        [Display(Name = "Mentor 4")]
        public string? Mentor4 { get; set; }

        [Display(Name = "Assigned Mentor")]
        public string? AssignedMentor { get; set; }

        [Display(Name = "GPA Overall")]
        public decimal? GPAOverall { get; set; }

        [Display(Name = "GPA Psychology")]
        public decimal? GPAPsych { get; set; }

        [Display(Name = "GPA Comments")]
        public string? GPAComment { get; set; }

        [Display(Name = "Course Requirements Met")]
        public bool? Course_Req_Met { get; set; }

        [Display(Name = "Course Requirements Comments")]
        public string? CrsReqComment { get; set; }

        [Display(Name = "Letter Quality")]
        public int? LetterQuality { get; set; }

        [Display(Name = "Letter Comments")]
        public string? LetterComment { get; set; }

        [Display(Name = "Resume Quality")]
        public int? ResumeQuality { get; set; }

        [Display(Name = "Resume Experience Quality")]
        public int? ResExpQuality { get; set; }

        [Display(Name = "Resume Comments")]
        public string? ResumeComment { get; set; }

        [Display(Name = "Writing Sample Quality")]
        public int? WritSampQuality { get; set; }

        [Display(Name = "Writing Sample Comments")]
        public string? WritSampComment { get; set; }

        [Display(Name = "Letter of Recommendation Relevance")]
        public int? LORRelevance { get; set; }

        [Display(Name = "Letter of Recommendation Quality")]
        public int? LORQuality { get; set; }

        [Display(Name = "Letter of Recommendation Comments")]
        public string? LORComment { get; set; }

        [Display(Name = "Overall Fit Quality")]
        public int? OverallFitQuality { get; set; }

        [Display(Name = "Overall Fit Comments")]
        public string? OverallFitComments { get; set; }

        [Display(Name = "Decision to Recommend")]
        public bool? DecRec { get; set; }

        [Display(Name = "Follow Up")]
        public bool? FollowUp { get; set; }

        [Display(Name = "Final Comments")]
        public string? FinalComments { get; set; }
    }
}
