using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace PGPARS.Models
{
    public class Review
    {
        [Key]
        public int? ReviewId { get; set; } //nullable ID

        public string? NNumber { get; set; } // n number

        public string? FirstName { get; set; } // first name

        public string? LastName { get; set; } // last name

        public DateTime? AppSubmitDate { get; set; } // date

        public string? Email { get; set; } // email

        public string? PhoneNumber { get; set; } // phone #

        public string? Status { get; set; } // should be 4 options

        public string? Reviewer { get; set; } // should be dropdown of ppl
        
        public double? AllGPA { get; set; } // overall GPA

        public double? PsychGPA { get; set; } // psych GPA

        public string? GPAComment { get; set; } //any comments about gpa

        public string? CourseReqMet { get; set; } 

        public string? CourseReqComments { get; set; }

        public int? LetterQuality { get; set; }

        public string? Mentor1 { get; set; }

        public string? Mentor2 { get; set; }

        public string? Mentor3 { get; set; }

        public string? LetterComment { get; set; }

        public int? ResumeQuality { get; set; }

        public int? ResExpQuality { get; set; }

        public string? ResumeComments { get; set; }

        public int? WritingSampleQuality { get; set; }

        public string? WritingSampleComments { get; set; }

        public int? LORRelevance {  get; set; }

        public int? LORQuality { get; set; }

        public string? LORComments { get; set; }

        public int? OverallFitQuality { get; set; }

        public string? OverallFitComments { get; set; }

        public string? DecRec { get; set; }

        public string? FollowUp { get; set; }

        public string? FinalComments { get; set; }

        public bool? IsAccepted { get; set; } //1. add the entire ass form like this, use alex and form for ref plz and ty

    }
}
