using Microsoft.AspNetCore.Mvc;
using PGPARS.Data;
using System.Linq;

namespace PGPARS.Controllers
{
    public class ApplicantController : Controller
    {
        private readonly IApplicantRepository _applicantRepository;

        // Constructor to inject the repository
        public ApplicantController(IApplicantRepository applicantRepository)
        {
            _applicantRepository = applicantRepository;
        }

        // Action to display the filtered list of applicants
        public IActionResult Applicants()
        {
            // Retrieve filtered applicants (Id, FullName, Gender, ApprovedStatus)
            var applicant = _applicantRepository.GetFilteredApplicants();

            // Pass the data to the view for display
            return View(applicant);
        }
    }
}
