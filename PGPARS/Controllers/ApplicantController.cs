using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PGPARS.Models;
using PGPARS.Data;
using PGPARS.Services;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using PGPARS.Models.ViewModels;

namespace PGPARS.Controllers
{
    public class ApplicantController : Controller
    {
        private readonly IApplicantRepository _applicantRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly AuditLogService _logger;

        public ApplicantController(IApplicantRepository applicantRepository, IWebHostEnvironment webHostEnvironment, AuditLogService auditLogService)
        {
            _applicantRepository = applicantRepository;
            _webHostEnvironment = webHostEnvironment;
            _logger = auditLogService;
        }

        public async Task<IActionResult> ApplicantDirectory(string searchString, int? cohort)
        {
            var applicants = await _applicantRepository.GetApplicantsAsync();

            // get all possible unique cohorts from repository
            var cohortYears = applicants
                .Select(a => a.Cohort)
                .Where(c => c.HasValue)
                .Distinct()
                .ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                applicants = applicants.Where(a => a.FullName.ToLower().Contains(searchString.ToLower())).ToList();
            }

            if (cohort != null)
            {
                applicants = applicants.Where(a => a.Cohort == cohort.Value).ToList();
            }

            // Obtain current year
            var year = DateTime.Now.Year;

            // Items to pass to the view
            ViewData["CurrentYear"] = year;
            ViewBag.CohortYears = cohortYears;
            ViewBag.Cohort = cohort;
            ViewData["SearchString"] = searchString;


            return View(applicants);
        }

        public async Task<IActionResult> ApplicantDetails(string Nnumber)
        {
            var applicants = await _applicantRepository.GetApplicantsAsync();
            var applicant = applicants.FirstOrDefault(a => a.Nnumber == Nnumber);

            if (applicant == null)
            {
                return NotFound();
            }

            return View(applicant);
        }

        [HttpGet]
        public async Task<IActionResult> EditApplicant(string Nnumber)
        {
            var applicants = await _applicantRepository.GetApplicantsAsync();
            var applicant = applicants.FirstOrDefault(a => a.Nnumber == Nnumber);

            if (applicant == null)
            {
                return NotFound();
            }

            var model = new ApplicantEditViewModel
            {
                Nnumber = applicant.Nnumber,
                FirstName = applicant.FirstName,
                LastName = applicant.LastName,
                AppSubmitDate = applicant.AppSubmitDate,
                Email = applicant.Email,
                Phone = applicant.Phone,
                AdvisorEmail = applicant.AdvisorEmail,
                Status = applicant.Status,
                Race = applicant.Race,
                Sex = applicant.Sex,
                Rev1Decision = applicant.Rev1Decision,
                Rev2Decision = applicant.Rev2Decision,
                RevAgree = applicant.RevAgree,
                Mentor1 = applicant.Mentor1,
                Mentor2 = applicant.Mentor2,
                Mentor3 = applicant.Mentor3,
                SelectMentor = applicant.SelectMentor,
                GPAOverall = applicant.GPAOverall,
                GPAPsych = applicant.GPAPsych,
                GPAComment = applicant.GPAComment,
                Course_Req_Met = applicant.Course_Req_Met,
                CrsReqComment = applicant.CrsReqComment
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditApplicant(ApplicantEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var applicants = await _applicantRepository.GetApplicantsAsync();
            var applicant = applicants.FirstOrDefault(a => a.Nnumber == model.Nnumber);

            if (applicant == null)
            {
                return NotFound();
            }

            applicant.Nnumber = model.Nnumber;
            applicant.FirstName = model.FirstName;
            applicant.LastName = model.LastName;
            applicant.AppSubmitDate = model.AppSubmitDate;
            applicant.Email = model.Email;
            applicant.Phone = model.Phone;
            applicant.AdvisorEmail = model.AdvisorEmail;
            applicant.Status = model.Status;
            applicant.Race = model.Race;
            applicant.Sex = model.Sex;
            applicant.Rev1Decision = model.Rev1Decision;
            applicant.Rev2Decision = model.Rev2Decision;
            applicant.RevAgree = model.RevAgree;
            applicant.Mentor1 = model.Mentor1;
            applicant.Mentor2 = model.Mentor2;
            applicant.Mentor3 = model.Mentor3;
            applicant.SelectMentor = model.SelectMentor;
            applicant.GPAOverall = model.GPAOverall;
            applicant.GPAPsych = model.GPAPsych;
            applicant.GPAComment = model.GPAComment;
            applicant.Course_Req_Met = model.Course_Req_Met;
            applicant.CrsReqComment = model.CrsReqComment;

            await _applicantRepository.SaveChangesAsync();
            await _logger.LogAction("Edit", User.Identity.Name, "Edited " + applicant.FullName, "APPLICANT");

            return RedirectToAction("ApplicantDetails", new { Nnumber = model.Nnumber });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSelectedApplicants(List<string> SelectedApplicants)
        {
            if (SelectedApplicants == null || !SelectedApplicants.Any())
            {
                return RedirectToAction("ApplicantDirectory");
            }

            foreach (var Nnumber in SelectedApplicants)
            {
                var applicants = await _applicantRepository.GetApplicantsAsync();
                var applicant = applicants.FirstOrDefault(a => a.Nnumber.Equals(Nnumber));

                if (applicant != null)
                {
                    _applicantRepository.DeleteApplicant(Nnumber);
                    await _logger.LogAction("Delete", User.Identity.Name, "Deleted " + applicant.FullName, "APPLICANT");
                }
            }
            TempData["SuccessMessage"] = "Applicants deleted";
            await _applicantRepository.SaveChangesAsync();

            return RedirectToAction("ApplicantDirectory");
        }
    }
}
