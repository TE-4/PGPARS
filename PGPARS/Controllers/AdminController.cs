using Microsoft.AspNetCore.Mvc;
using PGPARS.Data;
using PGPARS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using PGPARS.Models.ViewModels;
using PGPARS.Services;

namespace PGPARS.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IApplicantRepository _applicantRepository;
        private readonly IFundingRepository _fundingRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IAuditRepository _auditRepository;

        public AdminController(IApplicantRepository applicantRepository, IFundingRepository fundingRepository, IReviewRepository reviewRepository, IAuditRepository auditRepository)
        {
            _applicantRepository = applicantRepository;
            _fundingRepository = fundingRepository;
            _reviewRepository = reviewRepository;
            _auditRepository = auditRepository;
        }

        
         
        public IActionResult AdminDashboard()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Fetch data from services/repositories
                var applicants = _applicantRepository.GetApplicants();
                var reviews = _reviewRepository.GetReviews();
                var auditlogs = _auditRepository.GetLogs();
                //var budgetRemaining = _fundingService.GetBudgetRemaining();
                //var fundings = _fundingRepository.GetFunding();

                // Create and populate the ViewModel
                var viewModel = new AdminDashboardViewModel
                {
                    Applicants = applicants,
                    Reviews = reviews,
                    AuditLogs = auditlogs,
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
            var applicants = _applicantRepository.GetApplicants();
            var statusCount = new Dictionary<string, int>
            {
                {"Unassigned", 0},
                {"1st Review", 0},
                {"Review Conflict", 0},
                {"Interview Pending", 0},
                {"Accepted", 0},
                {"Funds Assigned", 0},
                {"Denied", 0}
            };

            foreach (var applicant in applicants)
            {
                if (applicant.Status != null && statusCount.ContainsKey(applicant.Status))
                {
                    statusCount[applicant.Status]++;
                }
            }

            var data = new
            {
                labels = statusCount.Keys.ToArray(),
                values = statusCount.Values.ToArray()
            };
            return Json(data);
        }

        
    }
}