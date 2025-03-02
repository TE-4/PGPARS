using PGPARS.Models;
using System.Collections.Generic;

namespace PGPARS.Data
{
    public interface IReviewRepository
    {
        IEnumerable<Review> GetReviews();
        Task<IEnumerable<Review>> GetReviewsAsync();
        Review GetReviewById(int ReviewId);
        void AddReview(Review review);
        void UpdateReview(Review review);
        void DeleteReview(int ReviewId);
    }
}
