using PGPARS.Models;
using PGPARS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PGPARS.Data;

namespace PGPARS.Controllers
{

    [Authorize(Roles = "Admin")]
    public class FileUploadController : Controller
    {
        private readonly CsvService _csvService;
        private readonly IApplicantRepository _applicantRepo;
        public FileUploadController(CsvService csvService, IApplicantRepository applicantRepo)
        {
            _csvService = csvService;
            _applicantRepo = applicantRepo;
        }
       
        public IActionResult ApplicantUpload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ApplicantUpload(IFormFile csvFile)
        {
            if (csvFile != null && csvFile.Length > 0)
            {
                using (var stream = csvFile.OpenReadStream())
                {
                    try
                    {
                        var applicants = _csvService.ReadCsvFile<Applicant>(stream, new ApplicantMap()).ToList();
                        // add applicants to the Applicant Table
                        _applicantRepo.AddApplicants(applicants);
                        return RedirectToAction("ApplicantDirectory", "Applicant");
                    }
                    catch (ApplicationException ex)
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                    }
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Please select a valid CSV file.");
            }
            return View();
        }
    }
}