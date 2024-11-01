using Microsoft.AspNetCore.Mvc;
using PGPARS.Data;

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
            var fundingList = _fundingRepository.GetFunding(); // Make sure this retrieves all funding records.
            return View(fundingList); // Ensure there is a FundingDirectory.cshtml file in Views/Funding
        }

    }


}

