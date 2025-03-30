using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PGPARS.Models
{
    public class FundingAllocation
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Funding")]
        public int FundingID { get; set; }
        public Funding? Funding { get; set; }

        [ForeignKey("Applicant")]
        public string Nnumber { get; set; }
        public Applicant? Applicant { get; set; }

        public decimal AllocatedAmount { get; set; }  // Amount of stipend awarded
        public decimal? StipendValue { get; set; }  // 1.0 = full, 0.5 = half

        public bool? TuitionWaiver { get; set; }  // Indicates if a tuition waiver is included
        public string? TuitionWaiverType { get; set; }  // "Full", "Half", "None", "Pending"

        public string? Status { get; set; }  // "Approved", "In Progress", "Denied"

        public string? AllocatedNotes { get; set; } //Notes or "Comment section" for the allocation to the student
    }
}
