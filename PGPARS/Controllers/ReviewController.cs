using PGPARS.Data;
using Microsoft.AspNetCore.Mvc;

namespace PGPARS.Controllers;
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewController(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }
        public IActionResult ReviewDirectory()
        {
            var reviewList = _reviewRepository.GetReviews();
            return View(reviewList);
        }
    }


