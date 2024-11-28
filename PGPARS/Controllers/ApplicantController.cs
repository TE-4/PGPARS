using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PGPARS.Models;
using PGPARS.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult ApplicantDirectory(string searchString)
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

        public IActionResult ApplicantDetails(string Nnumber)
        {
            var applicant = _applicantRepository.GetApplicants().FirstOrDefault(a => a.Nnumber == Nnumber);

            if (applicant == null)
            {
                return NotFound();
            }

            return View(applicant);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public Task<IActionResult> EditApplicant(string Nnumber)
        {
            var applicant = _applicantRepository.GetApplicants().FirstOrDefault(a => a.Nnumber == Nnumber);
            if (applicant == null)
            {
                return Task.FromResult<IActionResult>(NotFound());
            }
            var model = new ApplicantEditViewModel
            {
                Nnumber = applicant.Nnumber,
                FirstName = applicant.FirstName,
                LastName = applicant.LastName,
                AppSubmitDate = applicant.AppSubmitDate.ToString(),
                email = applicant.email,
                Phone = applicant.Phone,
                AdvisorEmail = applicant.AdvisorEmail,
                Status = applicant.Status,
                Race = applicant.Race,
                Sex = applicant.Sex,
                Reviewer1 = applicant.Reviewer1,
                Reviewer2 = applicant.Reviewer2,
                Rev1Decision = applicant.Rev1Decision,
                Rev2Decision = applicant.Rev2Decision,
                RevAgree = applicant.RevAgree.GetValueOrDefault().ToString(),
                CommitteeReview = applicant.CommitteeReview,
                Mentor1 = applicant.Mentor1,
                Mentor2 = applicant.Mentor2,
                Mentor3 = applicant.Mentor3,
                SelectMentor = applicant.SelectMentor,
                GPAOverall = applicant.GPAOverall.ToString(),
                GPAPsych = applicant.GPAPsych.ToString(),
                GPAComment = applicant.GPAComment,
                Course_Req_Met = applicant.Course_Req_Met.GetValueOrDefault().ToString(),
                CrsReqComment = applicant.CrsReqComment,
                LetterQuality = applicant.LetterQuality.ToString(),
                LetterComment = applicant.LetterComment,
                ResumeQuality = applicant.ResumeQuality.ToString(),
                ResExpQuality = applicant.ResExpQuality.ToString(),
                ResumeComment = applicant.ResumeComment,
                WritSampQuality = applicant.WritSampQuality.ToString(),
                WritSampComment = applicant.WritSampComment,
                LORRelevance = applicant.LORRelevance.ToString(),
                LORQuality = applicant.LORQuality.ToString(),
                LORComment = applicant.LORComment,
                OverallFitQuality = applicant.OverallFitQuality.ToString(),
                OverallFitComments = applicant.OverallFitComments,
                DecRec = applicant.DecRec,
                FollowUp = applicant.FollowUp,
                FinalComments = applicant.FinalComments
            };
            return Task.FromResult<IActionResult>(View(model));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> EditApplicant(ApplicantEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Task.FromResult<IActionResult>(View(model));
            }

            var applicant = _applicantRepository.GetApplicants().FirstOrDefault(a => a.Nnumber == model.Nnumber);
            if (applicant == null)
            {
                return Task.FromResult<IActionResult>(NotFound());
            }

            // Update Applicant details
            applicant.Nnumber = model.Nnumber;
            applicant.FirstName = model.FirstName;
            applicant.LastName = model.LastName;
            //applicant.AppSubmitDate = model.AppSubmitDate;
            applicant.email = model.email;
            applicant.Phone = model.Phone;
            applicant.AdvisorEmail = model.AdvisorEmail;
            applicant.Status = model.Status;
            applicant.Race = model.Race;
            applicant.Sex = model.Sex;
            applicant.Reviewer1 = model.Reviewer1;
            applicant.Reviewer2 = model.Reviewer2;
            applicant.Rev1Decision = model.Rev1Decision;
            //applicant.RevAgree = model.RevAgree;
            applicant.CommitteeReview = model.CommitteeReview;
            applicant.Mentor1 = model.Mentor1;
            applicant.Mentor2 = model.Mentor2;
            applicant.Mentor3 = model.Mentor3;
            applicant.SelectMentor = model.SelectMentor;
            //applicant.GPAOverall = model.GPAOverall;
            //applicant.GPAPsych = model.GPAPsych;
            applicant.GPAComment = model.GPAComment;
            //applicant.Course_Req_Met = model.Course_Req_Met;
            applicant.CrsReqComment = model.CrsReqComment;
            //applicant.LetterQuality = model.LetterQuality;
            applicant.LetterComment = model.LetterComment;
            //applicant.ResumeQuality = model.ResumeQuality;
            //applicant.ResExpQuality = model.ResExpQuality;
            applicant.ResumeComment = model.ResumeComment;
            //applicant.WritSampQuality = model.WritSampQuality;
            applicant.WritSampComment = model.WritSampComment;
            //applicant.LORRelevance = model.LORRelevance;
            //applicant.LORQuality = model.LORQuality;
            applicant.LORComment = model.LORComment;
            //applicant.OverallFitQuality = model.OverallFitQuality;
            applicant.OverallFitComments = model.OverallFitComments;
            applicant.DecRec = model.DecRec;
            applicant.FollowUp = model.FollowUp;
            applicant.FinalComments = model.FinalComments;


            // Update using the repository method
            _applicantRepository.UpdateApplicant(applicant);

            TempData["ApplicantUpdated"] = "Applicant Details successfully updated!";
            return Task.FromResult<IActionResult>(RedirectToAction("ApplicantDetails", new { Nnumber = model.Nnumber }));
        }
    } // END CLASS
}
