using PGPARS.Models;
using System.Collections.Generic;
using System.Linq;

namespace PGPARS.Data
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Review> GetReviews()
        {
            return _context.Reviews.ToList();
        }

        public Review GetReviewById(int reviewId)
        {
            return _context.Reviews.FirstOrDefault(r => r.ReviewNumber == reviewId);
        }

        public void AddReview(Review review)
        {
            _context.Reviews.Add(review);
            _context.SaveChanges(); // Commit the transaction to the database
        }

        public void UpdateReview(Review review)
        {
            var existingReview = _context.Reviews.FirstOrDefault(r => r.ReviewNumber == review.ReviewNumber);
            if (existingReview != null)
            {
                existingReview.Status = review.Status;
                existingReview.FullName = review.FullName;
                existingReview.Email = review.Email;
                existingReview.PhoneNumber = review.PhoneNumber;
                existingReview.AllGPA = review.AllGPA;
                existingReview.PsychGPA = review.PsychGPA;
                // Update additional fields as needed
                _context.SaveChanges();
            }
        }

        public void DeleteReview(int reviewId)
        {
            var review = _context.Reviews.FirstOrDefault(r => r.ReviewNumber == reviewId);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                _context.SaveChanges();
            }
        }
    }
}