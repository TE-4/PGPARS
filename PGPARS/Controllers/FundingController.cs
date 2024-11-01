using Microsoft.AspNetCore.Mvc;
using PGPARS.Data;
using PGPARS.Models.ViewModels;

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

        public IActionResult FundingDirectory()
        {
            var fundingList = _fundingRepository.GetFunding(); 
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

