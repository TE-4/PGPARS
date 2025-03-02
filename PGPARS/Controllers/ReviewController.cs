using Microsoft.AspNetCore.Mvc;
using PGPARS.Models;
using PGPARS.Data;
using System.Threading.Tasks;

namespace PGPARS.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IApplicantRepository _applicantRepository;

        public ReviewController(IReviewRepository reviewRepository, IApplicantRepository applicantRepository)
        {
            _reviewRepository = reviewRepository;
            _applicantRepository = applicantRepository;
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
    }
}
