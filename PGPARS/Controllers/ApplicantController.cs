﻿using Microsoft.AspNetCore.Authorization;
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
using PGPARS.Services;

namespace PGPARS.Controllers
{
    public class ApplicantController : Controller
    {
        private readonly IApplicantRepository _applicantRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly AuditLogService _logger;

        // Constructor to inject the repository and IWebHostEnvironment
        public ApplicantController(IApplicantRepository applicantRepository, IWebHostEnvironment webHostEnvironment, AuditLogService auditLogService)
        {
            _applicantRepository = applicantRepository;
            _webHostEnvironment = webHostEnvironment;
            _logger = auditLogService;
        }

        // Action to display the filtered list of applicants
        public IActionResult ApplicantDirectory(string searchString)
        {
            // Fetch all applicants from the repository
            var applicants = _applicantRepository.GetApplicants();

            // If the search string is not empty, filter the applicants by full name
            if (!string.IsNullOrEmpty(searchString))
            {
                applicants = applicants.Where(a => a.FullName.ToLower().Contains(searchString.ToLower())).ToList();
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
        public IActionResult EditApplicant(string Nnumber)
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditApplicant(ApplicantEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var applicant = _applicantRepository.GetApplicants().FirstOrDefault(a => a.Nnumber == model.Nnumber);
            if (applicant == null)
            {
                return NotFound();
            }

            // Update Applicant details
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


            // Update using the repository method
            _applicantRepository.UpdateApplicant(applicant);

            await _logger.LogAction("Edit", User.Identity.Name, "Edited " + applicant.FullName, "APPLICANT");
            return RedirectToAction("ApplicantDetails", new { Nnumber = model.Nnumber });
        }

        /*[Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteApplicant(string Nnumber)
        {
            var applicant = _applicantRepository.GetApplicants().FirstOrDefault(a => a.Nnumber == Nnumber);
            if (applicant != null)
            {
                _applicantRepository.DeleteApplicant(Nnumber); // Delete the applicant from the repository
            }
            
            return RedirectToAction("ApplicantDirectory");
        }*/

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
                var applicant = _applicantRepository.GetApplicants().FirstOrDefault(a => a.Nnumber.Equals(Nnumber));
                if (applicant != null)
                {
                    _applicantRepository.DeleteApplicant(Nnumber);
                    await _logger.LogAction("Delete", User.Identity.Name, "Deleted " + applicant.FullName, "APPLICANT");
                }
            }
            return RedirectToAction("ApplicantDirectory");
        }
    }
}
