namespace PGPARS.Models
{
    public class FundingAllocations
    {
      
            public int Id { get; set; }
            public int FundingSourceId { get; set; }
            public string? ApplicantId { get; set; }
            public decimal AllocatedAmount { get; set; }

            // Ensure you have a navigation property
            public FundingSource? FundingSource { get; set; }

            // You can access the name through the related FundingSource
            public string FundingSourceName => FundingSource?.Name;
        

        public ICollection<FundingUsage> UsageRecords { get; set; } // Tracks spending
    }
}
