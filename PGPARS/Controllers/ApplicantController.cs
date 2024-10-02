using Microsoft.AspNetCore.Mvc;
using PGPARS.Data;
using System.Linq;

namespace PGPARS.Controllers
{
    public class ApplicantController : Controller
    {
        private readonly ApplicantRepository _applicantRepository;

        // Constructor to inject the repository
        public ApplicantController(ApplicantRepository applicantRepository)
        {
            _applicantRepository = applicantRepository;
        }

        // Action to display the filtered list of applicants
        public IActionResult StudentDirectory()
        {
            // Retrieve filtered applicants (Id, FullName, Gender, ApprovedStatus)
            var applicant = _applicantRepository.GetFilteredApplicants();

            // Pass the data to the view for display
            return View(applicant);

        }
    }
}
