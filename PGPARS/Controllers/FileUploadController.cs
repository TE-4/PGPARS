using PGPARS.Models;
using PGPARS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PGPARS.Data;
using Microsoft.AspNetCore.Identity;

namespace PGPARS.Controllers
{

    [Authorize(Roles = "Admin")]
    public class FileUploadController : Controller
    {
        private readonly CsvService _csvService;
        private readonly IApplicantRepository _applicantRepo;
        private readonly UserManager<AppUser> _userManager;

        public FileUploadController(CsvService csvService, IApplicantRepository applicantRepo, UserManager<AppUser> userManager)
        {
            _csvService = csvService;
            _applicantRepo = applicantRepo;
            _userManager = userManager;
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

        public IActionResult FacultyUpload()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> FacultyUpload(IFormFile csvFile)
        {
            if (csvFile == null || csvFile.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "Please select a valid CSV file.");
                return View();
            }

            var errors = new List<string>();

            using (var stream = csvFile.OpenReadStream())
            {
                try
                {
                    var faculties = _csvService.ReadCsvFile<AppUser>(stream, new FacultyMap()).ToList();

                    foreach (var faculty in faculties)
                    {
                        // Check if user with the same email already exists
                        var existingUser = await _userManager.FindByEmailAsync(faculty.Email);
                        if (existingUser != null)
                        {
                            errors.Add($"A user with the email {faculty.Email} already exists.");
                            continue;
                        }

                        // Set UserName if not already populated
                        if (string.IsNullOrWhiteSpace(faculty.UserName))
                        {
                            faculty.UserName = faculty.Email;
                        }

                        // Create the user
                        var result = await _userManager.CreateAsync(faculty, "Password1234");
                        if (!result.Succeeded)
                        {
                            errors.Add($"Failed to create user {faculty.Email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                            continue;
                        }

                        // Assign the "Faculty" role
                        var roleResult = await _userManager.AddToRoleAsync(faculty, "Faculty");
                        if (!roleResult.Succeeded)
                        {
                            errors.Add($"Failed to assign role to user {faculty.Email}: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                        }
                    }

                    if (errors.Any())
                    {
                        ModelState.AddModelError(string.Empty, string.Join("<br>", errors));
                        return View();
                    }

                    return RedirectToAction("Directory", "Account");
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

            return View();
        }


    } // end class
} // end namespace