using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PGPARS.Data;
using PGPARS.Models;
using PGPARS.Models.ViewModels;
using PGPARS.Services;
using System.Collections.Generic;
using System.Linq;


namespace PGPARS.Controllers
{
    public class FundingController : Controller
    {
        private readonly IFundingRepository _fundingRepository;
        private readonly IApplicantRepository _applicantRepository;
        private readonly AuditLogService _logger;
        

        public FundingController(IFundingRepository fundingRepository, IApplicantRepository applicantRepository, AuditLogService auditLogService)
        {
            _fundingRepository = fundingRepository;
            _applicantRepository = applicantRepository;
            _logger = auditLogService;
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
                _logger.LogAction("Add", User.Identity.Name, "Added " + funding.Source, "INFO");
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

                //log for editing
                _logger.LogAction("Edit", User.Identity.Name, "Edited " + funding.Source, "INFO");

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
            _logger.LogAction("Delete", User.Identity.Name, "Deleted " + funding.Source, "INFO");
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
            if (funding == null || funding.Remaining < model.Amount)
            {
                ModelState.AddModelError("", "Insufficient funds or invalid funding source.");
                return View(model);
            }

            // Create new allocation
            var allocation = new FundingAllocations
            {
                FundingID = model.FundingSourceId,
                Nnumber = model.ApplicantId,
                AllocatedAmount = model.Amount
            };

            _fundingRepository.AddAllocation(allocation);

            // Update remaining funding amount
            funding.Remaining -= model.Amount;
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
            var applicants = _applicantRepository.GetApplicants();
            var model = new FundingAssignmentViewModel
            {
                FundingSourceId = funding.Id,
                FundingSourceName = funding.Source,
                Applicants = applicants,
                RemainingAmount = (decimal)funding.Remaining
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
