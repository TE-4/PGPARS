using PGPARS.Models;
using PGPARS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PGPARS.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ClosedXML.Excel;

namespace PGPARS.Controllers
{

    [Authorize(Roles = "Admin")]
    public class FileUploadController : Controller
    {
        private readonly CsvService _csvService;
        private readonly IApplicantRepository _applicantRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly AuditLogService _logger;

        public FileUploadController(CsvService csvService, IApplicantRepository applicantRepo, UserManager<AppUser> userManager, 
            AuditLogService als)
        {
            _csvService = csvService;
            _applicantRepo = applicantRepo;
            _userManager = userManager;
            _logger = als;
        }

        [HttpPost]
        public async Task<IActionResult> ApplicantUpload(IFormFile fileUpload, int cohort)
        {
            if (fileUpload == null || fileUpload.Length == 0)
            {
                TempData["ErrorMessage"] = "Please select a file.";
                return RedirectToAction("ApplicantDirectory", "Applicant");
            }

            var extension = Path.GetExtension(fileUpload.FileName).ToLower();

            try
            {
                List<Applicant> applicants = new();

                if (extension == ".csv")
                {
                    using var stream = fileUpload.OpenReadStream();
                    applicants = _csvService.ReadCsvFile<Applicant>(stream, new ApplicantMap()).ToList();

                    // for CSV uploads, ensure the selected cohort is applied to all applicants in the list
                    foreach(var applicant in applicants)
                    {
                        applicant.Cohort = cohort;
                    }
                }
                else if (extension == ".xlsx")
                {
                    using var stream = new MemoryStream();
                    await fileUpload.CopyToAsync(stream);
                    stream.Position = 0;

                    using var workbook = new XLWorkbook(stream);
                    var worksheet = workbook.Worksheet(1);
                    var rows = worksheet.RangeUsed().RowsUsed().Skip(1); // skip header row

                    foreach (var row in rows)
                    {
                        var applicant = new Applicant
                        {
                            Nnumber = row.Cell(1).GetValue<string>(),
                            FirstName = row.Cell(2).GetValue<string>(),
                            LastName = row.Cell(3).GetValue<string>(),
                            Email = row.Cell(4).GetValue<string>(),
                            Phone = row.Cell(5).GetValue<string>(),
                            PrimaryCitizenship = row.Cell(6).GetValue<string>(),
                            CitizenshipStatus = row.Cell(7).GetValue<string>(),
                            Status = row.Cell(11).GetValue<string>(),
                            Cohort = cohort,

                            // add more later as needed
                        };
                        applicants.Add(applicant);
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Unsupported file type. Please upload a .csv or .xlsx file.";
                    return RedirectToAction("ApplicantDirectory", "Applicant");
                }

                var uploadCount = _applicantRepo.AddApplicants(applicants);
                await _applicantRepo.SaveChangesAsync();

                foreach (var applicant in applicants)
                {
                    await _logger.LogAction("Upload", User.Identity.Name, applicant.FullName, "APPLICANT");
                }

                TempData["SuccessMessage"] = uploadCount == 0
                    ? "No new applicants added."
                    : $"{uploadCount} applicants have been added successfully.";

                await _logger.LogAction("Upload", User.Identity.Name, $"{uploadCount} applicants uploaded", "INFO");
            }
            catch (ApplicationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            return RedirectToAction("ApplicantDirectory", "Applicant");
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

            

            using (var stream = csvFile.OpenReadStream())
            {
                try
                {
                    var faculties = _csvService.ReadCsvFile<AppUser>(stream, new FacultyMap()).ToList();
                    int uploadCount = 0; // This is to keep track of how many users are uploaded successfully for the Toastr notification

                    foreach (var faculty in faculties)
                    {
                        // check if the user already exists in our AspNetUsers table
                        var userExists = await _userManager.Users.FirstOrDefaultAsync(u => u.Nnumber == faculty.Nnumber);

                        // if the user does already exist, we skip this iteration in our loop to prevent duplicates in our database
                        if (userExists != null)
                        {                            
                            continue;
                        }

                        faculty.UserName = faculty.Email;                   

                        var result = await _userManager.CreateAsync(faculty, "Password123!");
                        if (!result.Succeeded)
                        {
                            continue;
                        }

                        var roleResult = await _userManager.AddToRoleAsync(faculty, "Faculty");
                        if (!roleResult.Succeeded)
                        {
                            Debug.WriteLine("Error adding role to user");
                        }
                        else
                        {
                            // if everything has succeeded this far, we can increment our upload count
                            uploadCount++;
                            // Log the action
                            await _logger.LogAction("Upload", User.Identity.Name, faculty.FirstName + " " + faculty.LastName, "ACCOUNT");
                        }
                    }

                  

                    TempData["SuccessMessage"] = $"{uploadCount} faculty members uploaded successfully.";

                    

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