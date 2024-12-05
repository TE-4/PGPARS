using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PGPARS.Data;
using PGPARS.Models;
using PGPARS.Models.ViewModels;
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
                _fundingRepository.UpdateFunding(funding); // Update the funding in the repository
                return RedirectToAction("FundingDirectory");
            }
            return View(funding); // Return the form with validation errors
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

        [HttpPost]
        public IActionResult AssignFundingToApplicant(int fundingId, int applicantId)
        {
            var funding = _fundingRepository.GetFundingById(fundingId);
            var applicant = _applicantRepository.GetApplicantById(applicantId);

            if (funding != null && applicant != null)
            {
                funding.Applicant = applicant.FullName;
                funding.ApplicantId = applicantId;
                _fundingRepository.UpdateFunding(funding);
            }

            return RedirectToAction("FundingDirectory");
        }

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

        public IActionResult Assign(int fundingId)
        {
            var funding = _fundingRepository.GetFundingById(fundingId);

            // Check if funding is null
            if (funding == null)
            {
                // Redirect to FundingDirectory or return a NotFound view if funding does not exist
                return RedirectToAction("FundingDirectory");
            }

            // Filter applicants to include only those with "Approved for Funding" status
            var applicants = _applicantRepository.GetApplicants()
                               .Where(a => a.Status == "Approved for Funding").ToList();

            // Check if applicants is empty (optional, based on your requirements)
            if (applicants == null || !applicants.Any())
            {
                // Redirect to FundingDirectory or show a message that no applicants are available
                return RedirectToAction("FundingDirectory");
            }

            var viewModel = new FundingAssignmentViewModel
            {
                Funding = funding,
                Applicants = applicants
            };

            return View(viewModel);


        }



    }


}

