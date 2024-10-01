namespace PGPARS.Models
{
    public class ApplicantDisplayViewModel
    {
        public int Id { get; set; }
        public required string FullName { get; set; }
        public required string Gender { get; set; }
        public bool ApprovedStatus { get; set; } // Assuming you have an 'ApprovedStatus' field
    }
}

