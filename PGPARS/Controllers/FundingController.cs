using Microsoft.AspNetCore.Mvc;
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


        // POST: AssignFundingToApplicant
        [HttpPost]
        public IActionResult AssignFunding(int fundingID)
        {
            var funding = _fundingRepository.GetFundingById(fundingID);
            var applicants = _applicantRepository.GetApplicants();

            if (funding == null || applicants == null || applicants.Count() == 0)
            {
                return RedirectToAction("FundingDirectory");
            }

            var model = new FundingAssignmentViewModel
            {
                FundingSourceId = funding.FundingID,
                FundingSourceName = funding.Source,
                Applicants = applicants,
                RemainingAmount = funding.RemainingAmount
            };

            return View(model);
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

        // GET: Assign
        [HttpPost]
        public IActionResult Assign(FundingAssignmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var funding = _fundingRepository.GetFundingById(model.FundingSourceId);
                var applicants = _applicantRepository.GetApplicants();

                model.Applicants = applicants;
                model.RemainingAmount = funding.RemainingAmount;

                return View(model); // Return with validation errors
            }

            var fundingSource = _fundingRepository.GetFundingById(model.FundingSourceId);
            var applicant = _applicantRepository.GetApplicantById(model.ApplicantId);

            if (fundingSource == null || applicant == null || fundingSource.RemainingAmount < model.Amount)
            {
                return RedirectToAction("FundingDirectory");
            }

            var allocation = new FundingAllocations
            {
                FundingSourceId = model.FundingSourceId,
                ApplicantId = model.ApplicantId,
                AllocatedAmount = model.Amount
            };

            _fundingRepository.AddAllocation(allocation);

            // Deduct from funding source
            fundingSource.RemainingAmount -= model.Amount;
            _fundingRepository.UpdateFunding(fundingSource);


            return RedirectToAction("FundingDirectory");
        }

    }
}
