using Microsoft.AspNetCore.Mvc;
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
                return RedirectToAction("FundingDirectory");
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
        // Commented out because Applicant is no longer part of the Funding model
        /*
        [HttpPost]
        public IActionResult AssignFundingToApplicant(int fundingId, int applicantId)
        {
            var funding = _fundingRepository.GetFundingById(fundingId);
            var applicant = _applicantRepository.GetApplicantById(applicantId);

            if (funding != null && applicant != null)
            {
                funding.Applicant = applicant;
                funding.Nnumber = applicant.Nnumber;
                _fundingRepository.UpdateFunding(funding);
            }

            return RedirectToAction("FundingDirectory");
        }
        */

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
        // Commented out because Applicant is no longer part of the Funding model
        /*
        public IActionResult Assign(int fundingId)
        {
            var funding = _fundingRepository.GetFundingById(fundingId);

            if (funding == null)
            {
                return RedirectToAction("FundingDirectory");
            }

            var applicants = _applicantRepository.GetApplicants()
                               .Where(a => a.Status == "Approved for Funding")
                               .ToList();

            if (applicants == null || !applicants.Any())
            {
                return RedirectToAction("FundingDirectory");
            }

            var viewModel = new FundingAssignmentViewModel
            {
                Funding = funding,
                Applicants = applicants
            };

            return View(viewModel);
        }
        */
    }
}
