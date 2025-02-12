namespace PGPARS.Models.ViewModels
{
    public class FundingAssignmentViewModel
    {

        public int FundingSourceId { get; set; }
        public string? FundingSourceName { get; set; }
        public IEnumerable<Applicant>? Applicants { get; set; }
        public decimal RemainingAmount { get; set; }
        public string? ApplicantId { get; set; } // Ensure this is a string to match the Nnumber type
        public decimal Amount { get; set; }
    }
}
