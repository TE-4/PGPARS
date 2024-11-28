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
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Application Submission Date")]
        public string AppSubmitDate { get; set; }

        [Display(Name = "Email")]
        public string email { get; set; }

        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [Display(Name = "Advisor Email")]
        public string AdvisorEmail { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Race")]
        public string Race { get; set; }

        [Display(Name = "Sex")]
        public string Sex { get; set; }

        [Display(Name = "Reviewer 1")]
        public string Reviewer1 { get; set; }

        [Display(Name = "Reviewer 2")]
        public string Reviewer2 { get; set; }

        [Display(Name = "Reveiwer 1 Decision")]
        public string Rev1Decision { get; set; }

        [Display(Name = "Reveiwer 2 Decision")]
        public string Rev2Decision { get; set; }

        [Display(Name = "Reveiwers Agree")]
        public string RevAgree { get; set; }

        [Display(Name = "Committee Review")]
        public string CommitteeReview { get; set; }

        [Display(Name = "Mentor 1")]
        public string Mentor1 { get; set; }

        [Display(Name = "Mentor 2")]
        public string Mentor2 { get; set; }

        [Display(Name = "Mentor 3")]
        public string Mentor3 { get; set; }

        [Display(Name = "Selected Mentor")]
        public string SelectMentor { get; set; }

        [Display(Name = "GPA Overall")]
        public string GPAOverall { get; set; }

        [Display(Name = "GPA Psychology")]
        public string GPAPsych { get; set; }

        [Display(Name = "GPA Comments")]
        public string GPAComment { get; set; }

        [Display(Name = "Course Requirements Met")]
        public string Course_Req_Met { get; set; }

        [Display(Name = "Course Requirements Comments")]
        public string CrsReqComment { get; set; }

        [Display(Name = "Letter Quality")]
        public string LetterQuality { get; set; }

        [Display(Name = "Letter Comments")]
        public string LetterComment { get; set; }

        [Display(Name = "Resume Quality")]
        public string ResumeQuality { get; set; }

        [Display(Name = "Resume Experience Quality")]
        public string ResExpQuality { get; set; }

        [Display(Name = "Resume Comments")]
        public string ResumeComment { get; set; }

        [Display(Name = "Writing Sample Quality")]
        public string WritSampQuality { get; set; }

        [Display(Name = "Writing Sample Comments")]
        public string WritSampComment { get; set; }

        [Display(Name = "Letter of Recommendation Relevance")]
        public string LORRelevance { get; set; }

        [Display(Name = "Letter of Recommendation Quality")]
        public string LORQuality { get; set; }

        [Display(Name = "Letter of Recommendation Comments")]
        public string LORComment { get; set; }

        [Display(Name = "Overall Fit Quality")]
        public string OverallFitQuality { get; set; }

        [Display(Name = "Overall Fit Comments")]
        public string OverallFitComments { get; set; }

        [Display(Name = "Decision to Recommend")]
        public string DecRec { get; set; }

        [Display(Name = "Follow Up")]
        public string FollowUp { get; set; }

        [Display(Name = "Final Comments")]
        public string FinalComments { get; set; }
    }
}
