using Microsoft.AspNetCore.Mvc;
using PGPARS.Data;
using PGPARS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using PGPARS.Models.ViewModels;

namespace PGPARS.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IApplicantRepository _applicantRepository;
        private readonly IFundingRepository _fundingRepository;
        private readonly IReviewRepository _reviewRepository;

        public AdminController(IApplicantRepository applicantRepository, IFundingRepository fundingRepository, IReviewRepository reviewRepository)
        {
            _applicantRepository = applicantRepository;
            _fundingRepository = fundingRepository;
            _reviewRepository = reviewRepository;
        }

        public IActionResult AdminDashboard()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Fetch data from services/repositories
                var applicants = _applicantRepository.GetApplicants();
                var reviews = _reviewRepository.GetReviews();
                //var budgetRemaining = _fundingService.GetBudgetRemaining();
                //var fundings = _fundingRepository.GetFunding();

                // Create and populate the ViewModel
                var viewModel = new AdminDashboardViewModel
                {
                    Applicants = applicants,
                    Reviews = reviews,
                    //BudgetRemaining = budgetRemaining,
                    //Fundings = fundings
                };
                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpGet]
        public JsonResult GetChartData()
        {
            var data = new
            {
                labels = new[] { "Unassigned", "Reviews Pending", "Review Conflict", "Interview Pending", "Accepted", "Denied", "Funding Assigned" },
                values = new[] { 12, 19, 3, 5, 2 }
            };
            return Json(data);
        }
    }
}