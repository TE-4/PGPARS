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
        public IActionResult AssignFunding(int fundingId)
        {
            var funding = _fundingRepository.GetFundingById(fundingId);
            var applicants = _applicantRepository.GetApplicants();

            if (funding == null || applicants == null || applicants.Count() == 0)
            {
                return RedirectToAction("FundingDirectory");
            }

            var model = new FundingAssignmentViewModel
            {
                FundingSourceId = funding.Id,
                FundingSourceName = funding.Name,
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

        public IActionResult Assign(int fundingId, int applicantId, decimal amount)
        {
            var funding = _fundingRepository.GetFundingById(fundingId);
            var applicant = _applicantRepository.GetApplicantById(applicantId);

            if (funding == null || applicant == null || funding.RemainingAmount < amount)
            {
                return RedirectToAction("FundingDirectory");
            }

            // Create a new funding allocation
            var allocation = new FundingAllocations
            {
                FundingSourceId = fundingId,
                ApplicantId = applicantId,
                AllocatedAmount = amount
            };

            _fundingRepository.AddAllocation(allocation);

            // Deduct from funding source
            funding.RemainingAmount -= amount;
            _fundingRepository.UpdateFunding(funding);
            _fundingRepository.Save(); // Ensure the update is committed

            return RedirectToAction("FundingDirectory");
        }



    }
}
