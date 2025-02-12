namespace PGPARS.Models
{
    public class FundingAllocations
    {
      
            public int Id { get; set; }
            public int FundingSourceId { get; set; }
            public string? ApplicantId { get; set; }
            public decimal AllocatedAmount { get; set; }
    }
}
