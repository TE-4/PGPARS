using Microsoft.AspNetCore.Mvc;
using PGPARS.Data;
using PGPARS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using PGPARS.Models.ViewModels;

namespace PGPARS.Controllers
{
   [Authorize]
    public class CommitteeController : Controller
    {
        private readonly IApplicantRepository _applicantRepository;
        private readonly IReviewRepository _reviewRepository;

        public CommitteeController(IApplicantRepository applicantRepository, IReviewRepository reviewRepository)
        {
            _applicantRepository = applicantRepository;
            _reviewRepository = reviewRepository;
        }
        public IActionResult CommitteeDashboard()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View("CommitteeDashboard", "Committee");
                /*
                // Fetch data from services/repositories
                var applicants = _applicantRepository.GetApplicants();
                var reviews = _reviewRepository.GetReviews();

                // Create and populate the ViewModel
                var viewModel = new CommitteeDashboardViewModel
                {
                    Applicants = applicants,
                    Reviews = reviews
                };
                return View(viewModel);*/
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
    }
}