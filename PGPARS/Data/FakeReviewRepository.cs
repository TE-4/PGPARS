using PGPARS.Models;
using System.Collections.Generic;
namespace PGPARS.Data
{
    public class FakeReviewRepository : IReviewRepository
    {
        private List<Review> _reviewList = new List<Review>
        {
            new Review { FullName = "Buh, Jimmy" , NNumber = "01234567", ReviewNumber = 1 , Status = "Accepted"}
        };
        public IEnumerable<Review> GetReviews()
        {
            return _reviewList;
        }
        public Review GetReviewById(int reviewId)
        {
            return _reviewList.FirstOrDefault(r => r.ReviewNumber == reviewId);
        }
        public void AddReview(Review review)
        {
            review.ReviewNumber = _reviewList.Max(r => r.ReviewNumber) + 1; //new id
            _reviewList.Add(review);
        }
        public void UpdateReview(Review review)
        {
            var existingReview = _reviewList.FirstOrDefault(r => r.ReviewNumber == review.ReviewNumber);
            if (existingReview != null)
            {
                existingReview.Status = review.Status;
            }
        }
        public void DeleteReview(int reviewId)
        {
            var review = _reviewList.FirstOrDefault(r => r.ReviewNumber == reviewId);
            if (review != null)
            {
                _reviewList.Remove(review);
            }
        }
    }
}
