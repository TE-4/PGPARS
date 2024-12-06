namespace PGPARS.Models.ViewModels
{
    public class CommitteeDashboardViewModel
    {
        public IEnumerable<Applicant> Applicants { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
    }
}
