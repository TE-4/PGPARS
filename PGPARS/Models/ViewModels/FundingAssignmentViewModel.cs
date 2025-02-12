namespace PGPARS.Models.ViewModels
{
    public class FundingAssignmentViewModel
    {
        public int FundingSourceId { get; set; }
        public string FundingSourceName { get; set; }
        public IEnumerable<Applicant>? Applicants { get; set; } // Add this to pass applicants
        public decimal RemainingAmount { get; set; }

        // Add these properties to match the form fields
        public string? ApplicantId { get; set; }  // Ensure this matches the Nnumber type
        public decimal Amount { get; set; }
    }
}
