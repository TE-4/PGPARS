namespace PGPARS.Models.ViewModels
{
    public class SubmitReviewViewModel
    {
        public AppUser Reviewer { get; set; }
        public Applicant Applicant { get; set; }

        // Review Information
        public int? LetterQuality { get; set; }
        public int? ResumeQuality { get; set; }
        public int? ResExpQuality { get; set; }
        public string? ResumeComments { get; set; }
        public int? WritingSampleQuality { get; set; }
        public string? WritingSampleComments { get; set; }
        public int? LORRelevance { get; set; }
        public int? LORQuality { get; set; }
        public string? LORComments { get; set; }
        public int? OverallFitQuality { get; set; }
        public string? OverallFitComments { get; set; }
        public string? DecisionRecommendation { get; set; }
        public bool? FollowUpRequired { get; set; }
        public string? FinalComments { get; set; }



    }
}
