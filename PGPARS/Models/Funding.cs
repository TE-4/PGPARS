using System.ComponentModel.DataAnnotations;

namespace PGPARS.Models
{
    public class Funding
    {
        [Key]
        public string? Name { get; set; }  // Name of corporation
        public int FundingID { get; set; }
        public double? Amount { get; set; } // amount that is fundable
        public string? Applicant { get; set; } // Applicant FullName

        public int? ApplicantId { get; set; } // Nullable ID for applicant if assigned
    }

}
