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
        public async Task<IActionResult> EditApplicant(string Nnumber)
        {
            var applicant = _applicantRepository.GetApplicants().FirstOrDefault(a => a.Nnumber == Nnumber);
            if (applicant == null)
            {
                return NotFound();
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
            return View(model);
        }


    } // END CLASS
}
