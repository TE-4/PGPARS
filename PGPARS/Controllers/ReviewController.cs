using Microsoft.AspNetCore.Mvc;
using PGPARS.Models;
using PGPARS.Data;
using System.Threading.Tasks;
using PGPARS.Services;
using Microsoft.AspNetCore.Authorization;

namespace PGPARS.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IApplicantRepository _applicantRepository;
        private readonly ReviewAssignmentService reviewAssignmentService;
        private readonly AuditLogService _logger;

        public ReviewController(IReviewRepository reviewRepository, IApplicantRepository applicantRepository, ReviewAssignmentService ras, AuditLogService als)
        {
            _reviewRepository = reviewRepository;
            _applicantRepository = applicantRepository;
            reviewAssignmentService = ras;
            _logger = als;
        }

        // GET: AddReview
        [HttpGet]
        public IActionResult AddReview()
        {
            return View(new Review()); // Blank form
        }

        // POST: AddReview
        [HttpPost]
        public async Task<IActionResult> AddReview(Review review)
        {
            if (ModelState.IsValid)
            {
                _reviewRepository.AddReview(review);
                await _reviewRepository.SaveChangesAsync(); 

                return RedirectToAction("ReviewDirectory");
            }
            return View(review); // Return with errors
        }

        // GET: EditReview
        [HttpGet]
        public async Task<IActionResult> EditReview(int id)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            var model = new Review
            {
                ReviewNumber = review.ReviewNumber,
                Nnumber = review.Nnumber,
                LetterQuality = review.LetterQuality,
                ResumeQuality = review.ResumeQuality,
                ResExpQuality = review.ResExpQuality,
                ResumeComments = review.ResumeComments,
                WritingSampleQuality = review.WritingSampleQuality,
                WritingSampleComments = review.WritingSampleComments,
                LORRelevance = review.LORRelevance,
                LORQuality = review.LORQuality,
                LORComments = review.LORComments,
                OverallFitQuality = review.OverallFitQuality,
                OverallFitComments = review.OverallFitComments,
                FinalComments = review.FinalComments,
                Applicant = review.Applicant,
            };

            return View(model); // Return with form for editing
        }

        // POST: EditReview
        [HttpPost]
        public async Task<IActionResult> EditReview(Review review)
        {
            if (ModelState.IsValid)
            {
                _reviewRepository.UpdateReview(review);
                await _reviewRepository.SaveChangesAsync(); 

                return RedirectToAction("ReviewDirectory");
            }
            return View(review);
        }
        public async Task<IActionResult> ViewReview(int id)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }
        // POST: DeleteReview
        [HttpPost]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(id);
            if (review != null)
            {
                _reviewRepository.DeleteReview(id);
                await _reviewRepository.SaveChangesAsync(); 
            }
            return RedirectToAction("ReviewDirectory");
        }

        public async Task<IActionResult> ReviewDirectory()
        {
            var reviewList = await _reviewRepository.GetReviewsAsync(); 
            return View(reviewList);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignReviewers()
        {
            try
            {
                await reviewAssignmentService.AssignReviewersAsync();
                await _logger.LogAction("Assign Reviewers", User.Identity.Name, "Reviewers automatically assigned", "REVIEW");
                TempData["SuccessMessage"] = "Reviewers have been assigned successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error assigning reviewers: {ex.Message}";
            }
            return RedirectToAction("ReviewDirectory");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> UnassignIncompleteReviews()
        {
            try
            {
                await reviewAssignmentService.UnassignIncompleteReviewsAsync();
                await _logger.LogAction("Unassign Incomplete Reviews", User.Identity.Name, "Incomplete reviews automatically unassigned", "REVIEW");
                TempData["SuccessMessage"] = "All incomplete reviews have been unassigned successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("ReviewDirectory");
        }
    }
}
