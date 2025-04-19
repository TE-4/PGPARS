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
        Task<List<Review>> GetReviewsByApplicantIdAsync(string ApplicantId);
        Task<List<AppUser>> GetReviewersByApplicantIdAsync(string ApplicantId);
        Task<List<Review>> GetReviewsByReviewerIdAsync(string ReviewerId);
        void AddReview(Review review);
        Task AddReviewAsync(Review review);
        Task AddReviewsAsync(List<Review> reviews);
        Task SaveChangesAsync();
        void UpdateReview(Review review);
        void DeleteReview(int ReviewId);

    }
}
