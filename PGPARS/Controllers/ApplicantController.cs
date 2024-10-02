using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using PGPARS.Data;
using System.Linq;

namespace PGPARS.Controllers
{
    public class ApplicantController : Controller
    {
        private readonly ApplicantRepository _applicantRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Constructor to inject the repository and IWebHostEnvironment
        public ApplicantController(ApplicantRepository applicantRepository, IWebHostEnvironment webHostEnvironment)
        {
            _applicantRepository = applicantRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        // Action to display the filtered list of applicants
        public IActionResult StudentDirectory()
        {
            var applicants = _applicantRepository.GetApplicants();
            return View(applicants);  // Pass the applicants to the view
        }

        [HttpPost]
        public IActionResult ImportApplicants()
        {
            // Get the physical path of the CSV file in wwwroot/Files
            string filePath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, "Files", "ApplicantMasterList.csv");

            _applicantRepository.ImportApplicantsFromCsv(filePath);

            return RedirectToAction("StudentDirectory");
        }
    }
}
