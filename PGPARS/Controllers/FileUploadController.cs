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
        private readonly IConfiguration _config;

        public FileUploadController(CsvService csvService, IApplicantRepository applicantRepo, UserManager<AppUser> userManager, 
            AuditLogService als, IConfiguration config)
        {
            _csvService = csvService;
            _applicantRepo = applicantRepo;
            _userManager = userManager;
            _logger = als;
            _config = config;
        }

        [HttpPost]
        public async Task<IActionResult> ApplicantUpload(IFormFile fileUpload, int cohort)
        {

            // first check if file actually has content, if not return with error message
            if (fileUpload == null || fileUpload.Length == 0)
            {
                TempData["ErrorMessage"] = "Please select a file.";
                return RedirectToAction("ApplicantDirectory", "Applicant");
            }

            // obtain the base URL for sharepoint PDF link from the config
            string? SharePointBaseUrl = _config["SharePoint:PdfBaseUrl"];

            // obtain the file extension to determine if need to parse CSV or Excel file
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

                // before saving to the database, add SharePoint URL with the applicant's Nnumber
                foreach(var applicant in applicants)
                {
                    applicant.SharePointPdfUrl = $"{SharePointBaseUrl}{applicant.Nnumber}.pdf";
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
        public async Task<IActionResult> FacultyUpload(IFormFile fileUpload)
        {
            if (fileUpload == null || fileUpload.Length == 0)
            {
                TempData["ErrorMessage"] = "Please select a valid file.";
                return RedirectToAction("Directory", "Account");
            }

            var extension = Path.GetExtension(fileUpload.FileName).ToLower();
            int uploadCount = 0;

            try
            {
                List<AppUser> faculties = new();

                if (extension == ".csv")
                {
                    using var stream = fileUpload.OpenReadStream();
                    faculties = _csvService.ReadCsvFile<AppUser>(stream, new FacultyMap()).ToList();
                }
                else if (extension == ".xlsx")
                {
                    using var stream = new MemoryStream();
                    await fileUpload.CopyToAsync(stream);
                    stream.Position = 0;

                    using var workbook = new XLWorkbook(stream);
                    var worksheet = workbook.Worksheet(1);
                    var rows = worksheet.RangeUsed().RowsUsed().Skip(1); // skip header

                    foreach (var row in rows)
                    {
                        var faculty = new AppUser
                        {
                            Nnumber = row.Cell(1).GetValue<string>(),
                            FirstName = row.Cell(2).GetValue<string>(),
                            LastName = row.Cell(3).GetValue<string>(),
                            Email = row.Cell(5).GetValue<string>(),
                            UserName = row.Cell(5).GetValue<string>(),
                            Position = row.Cell(6).GetValue<string>(),
                             
                            // can add more if needed
                        };

                        faculties.Add(faculty);
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Unsupported file type. Please upload a .csv or .xlsx file.";
                    return RedirectToAction("Directory", "Account");
                }

                foreach (var faculty in faculties)
                {
                    var userExists = await _userManager.Users.FirstOrDefaultAsync(u => u.Nnumber == faculty.Nnumber);
                    if (userExists != null)
                        continue;

                    var result = await _userManager.CreateAsync(faculty, "Password123!");
                    if (!result.Succeeded)
                        continue;

                    var roleResult = await _userManager.AddToRoleAsync(faculty, "Faculty");
                    if (roleResult.Succeeded)
                    {
                        uploadCount++;
                        await _logger.LogAction("Upload", User.Identity.Name, $"{faculty.FirstName} {faculty.LastName}", "ACCOUNT");
                    }
                }

                TempData["SuccessMessage"] = uploadCount == 0
                    ? "No new faculty members added."
                    : $"{uploadCount} faculty members uploaded successfully.";

                await _logger.LogAction("Upload", User.Identity.Name, $"{uploadCount} faculty users uploaded", "INFO");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            return RedirectToAction("Directory", "Account");
        }



    } // end class
} // end namespace