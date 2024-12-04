using Microsoft.AspNetCore.Mvc;
using PGPARS.Data;
using PGPARS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;

namespace PGPARS.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IApplicantRepository _applicantRepository;
        private readonly IFundingRepository _fundingRepository;
        private readonly IReviewRepository _reviewRepository;

        public AdminController()
        {
            //_applicantRepository = applicantRepository;

        }

        public IActionResult Dashboard()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
    }
}