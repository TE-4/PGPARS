namespace PGPARS.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public IEnumerable<Applicant> Applicants { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
        public IEnumerable<Funding> Fundings { get; set; }
        //public decimal BudgetRemaining { get; set; }
    }
}
