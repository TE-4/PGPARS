namespace PGPARS.Models
{
    public class FundingSource
    {
        public int FundingSourceId { get; set; }
        public string Name { get; set; } // Scholarship A, Grant B, etc.
        public decimal TotalAmount { get; set; } // Original fund size
        public decimal RemainingAmount { get; set; } // Updates after allocations

        public ICollection<FundingAllocations> Allocations { get; set; } // Links to allocations
    }

}
