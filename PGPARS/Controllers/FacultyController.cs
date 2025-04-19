using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PGPARS.Data;
using PGPARS.Models.ViewModels;
using PGPARS.Models;

namespace PGPARS.Controllers
{
    public class FacultyController : Controller
    {
        private readonly IApplicantRepository _applicantRepository;
        private readonly IFundingRepository _fundingRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IAuditRepository _auditRepository;
        private readonly UserManager<AppUser> _userManager;

        public FacultyController(IApplicantRepository applicantRepository, IReviewRepository reviewRepository, 
            IAuditRepository auditRepository, UserManager<AppUser> userManager)
        {
            _applicantRepository = applicantRepository;
            _reviewRepository = reviewRepository;
            _auditRepository = auditRepository;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles = "Faculty, Admin, Committee")]
        public async Task<IActionResult> Dashboard()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Fetch data from services/repositories
                var applicants = _applicantRepository.GetApplicants();
                var auditlogs = await _auditRepository.GetLogsAsync();


                // get currently logged in User
                var user = await _userManager.GetUserAsync(User);

                // get reviews assigned to logged in user
                var reviews = await _reviewRepository.GetReviewsByReviewerIdAsync(user.Id);

                

                // get total reviews and completed reviews for logged in user
                var totalReviews = reviews.Count();
                var completedReviews = reviews.Count(r => r.ReviewComplete);

                // filter out the completed reviews so they only see the reviews that are not completed
                reviews = reviews.Where(r => r.ReviewComplete == false).ToList();

                // Create and populate the ViewModel
                var viewModel = new FacultyDashboardViewModel
                {
                    Applicants = applicants,
                    Reviews = reviews,
                    AuditLogs = auditlogs,
                    TotalReviews = totalReviews,
                    CompletedReviews = completedReviews

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
