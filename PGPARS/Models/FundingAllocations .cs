namespace PGPARS.Models
{
    public class FundingAllocations
    {
        public int Id { get; set; }
        public int FundingSourceId { get; set; }
        public string? ApplicantId { get; set; } // Ensure it's a string if storing Nnumber
        public decimal AllocatedAmount { get; set; } // Amount given to applicant

        public ICollection<FundingUsage> UsageRecords { get; set; } // Tracks spending
    }
}
