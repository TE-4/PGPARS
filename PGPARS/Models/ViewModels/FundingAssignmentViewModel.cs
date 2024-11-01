using PGPARS.Models;

public class FundingAssignmentViewModel
{
    public Funding Funding { get; set; } // Funding details
    public IEnumerable<Applicant> Applicants { get; set; } // List of applicants to choose from
}
