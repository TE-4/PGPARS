using Microsoft.AspNetCore.Mvc;
using PGPARS.Data;

namespace PGPARS.Controllers
{
    public class FundingController : Controller
    {
        private readonly IFundingRepository _fundingRepository;

        public FundingController(IFundingRepository fundingRepository)
        {
            _fundingRepository = fundingRepository;
        }

        public IActionResult FundingDirectory()
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
    }
}
