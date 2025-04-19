using System.ComponentModel.DataAnnotations;

namespace PGPARS.Models.ViewModels
{
    public class SubmitReviewViewModel
    {
        [Required]
        public int ReviewNumber { get; set; }

        [Required]
        [Range(1, 5)]
        public int? ResumeQuality { get; set; }

        [Required]
        [Range(1, 5)]
        public int? ResExpQuality { get; set; }

        public string? ResumeComments { get; set; }

        [Required]
        [Range(1, 5)]
        public int? LetterQuality { get; set; }

        public string? LetterComments { get; set; }

        [Required]
        [Range(1, 5)]
        public int? LORRelevance { get; set; }

        [Required]
        [Range(1, 5)]
        public int? LORQuality { get; set; }

        public string? LORComments { get; set; }

        [Required]
        [Range(1, 5)]
        public int? WritingSampleQuality { get; set; }

        public string? WritingSampleComments { get; set; }

        [Required]
        [Range(1, 5)]
        public int? OverallFitQuality { get; set; }

        public string? OverallFitComments { get; set; }

        [Required]
        public bool? FollowUpRequired { get; set; }

        [Required]
        public string DecisionRecommendation { get; set; }

        public string? FinalComments { get; set; }

        // Optional display-only data
        public string? ApplicantFullName { get; set; }
        public string? Nnumber { get; set; }
    }
}
