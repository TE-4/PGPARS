namespace PGPARS.Models
{
    public class FundingAllocations
    {
      public int Id { get; set; }
        public int FundingSourceId { get; set; }
        public FundingSource FundingSource { get; set; } // Links back to source

        public int ApplicantId { get; set; }
        public Applicant Applicant { get; set; } // Links to applicants

        public decimal AllocatedAmount { get; set; } // Amount given to applicant

        public ICollection<FundingUsage> UsageRecords { get; set; } // Tracks spending
    }
}
