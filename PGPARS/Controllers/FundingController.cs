﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PGPARS.Data;
using PGPARS.Models;
using PGPARS.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;


namespace PGPARS.Controllers
{
    public class FundingController : Controller
    {
        private readonly IFundingRepository _fundingRepository;
        private readonly IApplicantRepository _applicantRepository;

        public FundingController(IFundingRepository fundingRepository, IApplicantRepository applicantRepository)
        {
            _fundingRepository = fundingRepository;
            _applicantRepository = applicantRepository;
        }

        // GET: AddFunding
        [HttpGet]
        public IActionResult AddFunding()
        {
            return View(new Funding()); // Return a blank form for adding funding
        }

        // POST: AddFunding
        [HttpPost]
        public IActionResult AddFunding(Funding funding)
        {
            if (ModelState.IsValid)
            {
                _fundingRepository.AddFunding(funding); // Add the funding to the repository
                return RedirectToAction("FundingDirectory"); // Redirect to FundingDirectory after successful submission
            }
            return View(funding); // Return the form with validation errors and user input
        }

        // GET: EditFunding
        [HttpGet]
        public IActionResult EditFunding(int id)
        {
            var funding = _fundingRepository.GetFundingById(id);
            if (funding == null)
            {
                return NotFound();
            }
            return View(funding); // Return the populated form for editing
        }

        // POST: EditFunding
        [HttpPost]
        public IActionResult EditFunding(Funding funding)
        {
            if (ModelState.IsValid)
            {
                // Call the repository method to update the funding in the database
                _fundingRepository.UpdateFunding(funding);

                // Redirect back to the directory after updating
                return RedirectToAction("FundingDirectory");
            }

            // If the model state is invalid, return to the edit page with errors
            return View(funding);
        }

        // POST: DeleteFunding
        [HttpPost]
        public IActionResult DeleteFunding(int id)
        {
            var funding = _fundingRepository.GetFundingById(id);
            if (funding != null)
            {
                _fundingRepository.DeleteFunding(id); // Delete the funding from the repository
            }
            return RedirectToAction("FundingDirectory");
        }


        // POST: Assign (Processes funding allocation)
        [HttpPost]
        public IActionResult Assign(FundingAssignmentViewModel model)
        {
            if (!ModelState.IsValid || model.ApplicantId == null || model.Amount <= 0)
            {
                ModelState.AddModelError("", "Invalid input. Please select an applicant and provide a valid amount.");
                return View(model); // Return to form with errors
            }

            var funding = _fundingRepository.GetFundingById(model.FundingSourceId);
            if (funding == null || funding.RemainingAmount < model.Amount)
            {
                ModelState.AddModelError("", "Insufficient funds or invalid funding source.");
                return View(model);
            }

            // Create new allocation
            var allocation = new FundingAllocations
            {
                FundingSourceId = model.FundingSourceId,
                ApplicantId = model.ApplicantId,
                AllocatedAmount = model.Amount
            };

            _fundingRepository.AddAllocation(allocation);

            // Update remaining funding amount
            funding.RemainingAmount -= model.Amount;
            _fundingRepository.UpdateFunding(funding);

            return RedirectToAction("FundingDirectory"); // Redirect to funding list
        }

        // GET: FundingDirectory
        public IActionResult FundingDirectory(string searchQuery)
        {
            IEnumerable<Funding> fundingList;

            if (!string.IsNullOrEmpty(searchQuery))
            {
                fundingList = _fundingRepository.SearchFunding(searchQuery);
            }
            else
            {
                fundingList = _fundingRepository.GetFunding();
            }

            ViewData["SearchQuery"] = searchQuery; // Preserve the search query in the view
            return View(fundingList);
        }

        // GET: Assign (Displays the assignment form)
        [HttpGet]
        public IActionResult Assign(int fundingId)
        {
            var funding = _fundingRepository.GetFundingById(fundingId);
            if (funding == null)
            {
                return RedirectToAction("FundingDirectory"); // Redirect if funding not found
            }

            var applicants = _applicantRepository.GetApplicants();
            var model = new FundingAssignmentViewModel
            {
                FundingSourceId = funding.FundingID,
                FundingSourceName = funding.Source,
                Applicants = applicants,
                RemainingAmount = (decimal)funding.RemainingAmount
            };

            return View(model); // Display the assignment form
        }


        [HttpGet]
        public JsonResult CheckApplicants(int fundingId)
        {
            var applicants = _applicantRepository.GetApplicants();
            bool hasApplicants = applicants != null && applicants.Any();
            return Json(new { hasApplicants });
        }

    }
}
