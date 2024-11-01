using PGPARS.Models;

public class FundingAssignmentViewModel
{
    public Funding Funding { get; set; }
    public IEnumerable<Applicant> Applicants { get; set; }
}
