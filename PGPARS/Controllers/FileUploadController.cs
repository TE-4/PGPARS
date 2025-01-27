using PGPARS.Models;
using PGPARS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PGPARS.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace PGPARS.Controllers
{

    [Authorize(Roles = "Admin")]
    public class FileUploadController : Controller
    {
        private readonly CsvService _csvService;
        private readonly IApplicantRepository _applicantRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public FileUploadController(CsvService csvService, IApplicantRepository applicantRepo, UserManager<AppUser> userManager)
        {
            _csvService = csvService;
            _applicantRepo = applicantRepo;
            _userManager = userManager;
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
                        TempData["SuccessMessage"] = $"{applicants.Count} applicants have been added successfully.";
                        return RedirectToAction("ApplicantDirectory", "Applicant");
                    }
                    catch (ApplicationException ex)
                    {
                        TempData["ErrorMessage"] = ex.Message;
                    }
                    catch (Exception ex)
                    {
                        TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                    }
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Please select a valid CSV file.");
            }
            return View();
        }




        [HttpPost]
        public async Task<IActionResult> FacultyUpload(IFormFile csvFile)
        {
            if (csvFile == null || csvFile.Length == 0)
            {
                // Return to the User Directory or display an error
                TempData["ErrorMessage"] = "Please select a valid CSV file.";
                return RedirectToAction("Directory", "Account"); 
            }

            var errors = new List<string>();

            using (var stream = csvFile.OpenReadStream())
            {
                try
                {
                    var faculties = _csvService.ReadCsvFile<AppUser>(stream, new FacultyMap()).ToList();

                    foreach (var faculty in faculties)
                    {
                        var userExists = await _userManager.Users.FirstOrDefaultAsync(u => u.Nnumber == faculty.Nnumber);
                        if (userExists != null)
                        {
                            errors.Add($"A user with the N-Number {faculty.Nnumber} already exists.");
                            continue;
                        }

                        faculty.UserName = faculty.Email;
                        faculty.MainRole = "Faculty";

                        var result = await _userManager.CreateAsync(faculty, "Password123!");
                        if (!result.Succeeded)
                        {
                            errors.Add($"Failed to create user {faculty.Email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                            continue;
                        }

                        var roleResult = await _userManager.AddToRoleAsync(faculty, "Faculty");
                        if (!roleResult.Succeeded)
                        {
                            errors.Add($"Failed to assign role to user {faculty.Email}: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                        }
                    }

                    if (errors.Any())
                    {
                        TempData["ErrorMessage"] = string.Join("<br>", errors);
                        return RedirectToAction("Directory", "Account"); 
                    }

                    TempData["SuccessMessage"] = $"{faculties.Count} faculty members uploaded successfully.";
                    return RedirectToAction("Directory", "Account"); 
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                    return RedirectToAction("Directory", "Account"); 
                }
            }
        }


    } // end class
} // end namespace