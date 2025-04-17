namespace PGPARS.Models.ViewModels
{
    public class FacultyDashboardViewModel
    {
        public int CurrentCohort { get; set; }
        public IEnumerable<Applicant> Applicants { get; set; }
        public IEnumerable<Review> Reviews { get; set; }

        public IEnumerable<AuditLog> AuditLogs { get; set; }

        // Add Interviews as well 
    }
}
