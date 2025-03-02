using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PGPARS.Models
{
    public class Review
    {
        [Key]
        public int ReviewNumber { get; set; } // Primary Key

        // Foreign Key to Applicant
        [ForeignKey("Applicant")]
        public string Nnumber { get; set; }  // Applicant's ID (FK)
        public Applicant Applicant { get; set; }  // Navigation Property

        // Foreign Key to AppUser (Committee Reviewer)
        [ForeignKey("AppUser")]
        public string AppUserId { get; set; }  // Reviewer ID (FK)
        public AppUser AppUser { get; set; }  // Navigation Property

        // Review Details
        public bool ReviewComplete { get; set; } = false; // Is the Review Complete?
        public DateTime? ReviewDate { get; set; }  // Date of the Review
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
