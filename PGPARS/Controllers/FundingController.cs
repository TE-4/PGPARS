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
            var fundingList = _fundingRepository.GetFunding();
            return View(fundingList);
        }
    }
}
