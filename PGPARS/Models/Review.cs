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

        // -- Review Details -- 

        // Auto assigned properties 
        public int Cohort { get; set; }  // Cohort Year
        public bool ReviewComplete { get; set; } = false; // Is the Review Complete?
        public DateTime? ReviewDate { get; set; }  // Date of the Review
        public DateTime? ReviewEdited { get; set; }  // Date of the Review Edit

        // -- Properties assigned from the Review Form -- 

        // Resume / Research
        public int? ResumeQuality { get; set; } // 1-5 (Quality of Resume)
        public int? ResExpQuality { get; set; } // 1-5 (Research Experience Quality)
        public string? ResumeComments { get; set; }

        // Letter (Statement of Purpose / Personal Statement)
        public int? LetterQuality { get; set; } // Letter refers to Statement of Purpose / Personal Statement
        public string? LetterComments { get; set; }

        // Letter of Recommendation
        public int? LORRelevance { get; set; } // LOR is Letter of Recommendation
        public int? LORQuality { get; set; }
        public string? LORComments { get; set; }

        // Writing Sample
        public int? WritingSampleQuality { get; set; }
        public string? WritingSampleComments { get; set; }

        // Overall Fit for Program
        public int? OverallFitQuality { get; set; }
        public string? OverallFitComments { get; set; }

        // Final Decision and follow up
        public bool? FollowUpRequired { get; set; }
        public string? DecisionRecommendation { get; set; }
        public string? FinalComments { get; set; }
    }
}
