namespace PGPARS.Models.ViewModels
{
    public class FacultyDashboardViewModel
    {
        public int CurrentCohort { get; set; }
        public IEnumerable<Applicant> Applicants { get; set; }
        public IEnumerable<Review> Reviews { get; set; }

        // Add Interviews as well 
    }
}
