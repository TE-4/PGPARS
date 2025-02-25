namespace PGPARS.Models.ViewModels
{
    public class FundingAssignmentViewModel
    {

        public int FundingId { get; set; }
        public IEnumerable<Applicant>? Applicants { get; set; } = new List<Applicant>();
        public string? ApplicantId { get; set; } 
        public decimal Amount { get; set; }
    }
}
