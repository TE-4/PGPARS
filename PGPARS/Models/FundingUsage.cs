namespace PGPARS.Models
{
    public class FundingUsage
    {
        public int Id { get; set; }
        public int FundingAllocationId { get; set; }
        public FundingAllocations FundingAllocations { get; set; }
        public decimal UsedAmount { get; set; } // Amount used from the allocation
        public DateTime DateUsed { get; set; }
        public string Comment { get; set; } // Description of usage (e.g., Tuition payment)
    }
}
