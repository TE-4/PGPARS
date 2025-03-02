using PGPARS.Models;
using System.Collections.Generic;

namespace PGPARS.Data
{
    public interface IReviewRepository
    {
        IEnumerable<Review> GetReviews();
        Task<IEnumerable<Review>> GetReviewsAsync();
        Review GetReviewById(int ReviewId);
        Task<Review> GetReviewByIdAsync(int ReviewId);
        void AddReview(Review review);
        Task AddReviewAsync(Review review);
        Task SaveChangesAsync();
        void UpdateReview(Review review);
        void DeleteReview(int ReviewId);
    }
}
