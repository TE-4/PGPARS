using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using PGPARS.Data;
using System.Linq;

namespace PGPARS.Controllers
{
    public class ApplicantController : Controller
    {
        private readonly IApplicantRepository _applicantRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Constructor to inject the repository and IWebHostEnvironment
        public ApplicantController(IApplicantRepository applicantRepository, IWebHostEnvironment webHostEnvironment)
        {
            _applicantRepository = applicantRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        // Action to display the filtered list of applicants
        public IActionResult StudentDirectory(string searchString)
        {
            // Fetch all applicants from the repository
            var applicants = _applicantRepository.GetApplicants();

            // If the search string is not empty, filter the applicants by full name
            if (!string.IsNullOrEmpty(searchString))
            {
                applicants = applicants.Where(a => a.FullName.Contains(searchString)).ToList();
            }

            // Pass the search string back to the view to retain the search term
            ViewData["SearchString"] = searchString;

            // Return the filtered (or full) list of applicants to the view
            return View(applicants);
        }

        public IActionResult ApplicantInfo()
        {
            return View();
        }
    }
}
