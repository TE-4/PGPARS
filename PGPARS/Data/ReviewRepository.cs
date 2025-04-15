using Microsoft.EntityFrameworkCore;
using PGPARS.Data;
using PGPARS.Models;

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

    public async Task<IEnumerable<Review>> GetReviewsAsync()
    {
        return await _context.Reviews
            .Include(r => r.Applicant)
            .Include(r => r.AppUser)
            .ToListAsync();
    }

    public Review GetReviewById(int reviewId)
    {
        return _context.Reviews.FirstOrDefault(r => r.ReviewNumber == reviewId);
    }

    public async Task<Review> GetReviewByIdAsync(int reviewId)
    {
        return await _context.Reviews.FirstOrDefaultAsync(r => r.ReviewNumber == reviewId);
    }


    public void AddReview(Review review)
    {
        _context.Reviews.Add(review);
    }

    public async Task<List<Review>> GetReviewsByApplicantIdAsync(string ApplicantId)
    {
        var reviews = await _context.Reviews
            .Include(r => r.Applicant)
            .Include(r => r.AppUser)
            .Where(r => r.Nnumber == ApplicantId)
            .ToListAsync();

        return reviews;
    }

    public async Task<List<AppUser>> GetReviewersByApplicantIdAsync(string ApplicantId)
    {
        var reviews = await _context.Reviews
            .Include(r => r.AppUser)
            .Where(r => r.Nnumber == ApplicantId)
            .Select(r => r.AppUser)
            .Distinct()
            .ToListAsync();
        return reviews;
    }

    
    public async Task AddReviewAsync(Review review)
    {
        await _context.Reviews.AddAsync(review);
    }

    public async Task AddReviewsAsync(List<Review> reviews)
    {
        await _context.Reviews.AddRangeAsync(reviews);
    }


    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void UpdateReview(Review review)
    {
        var existingReview = _context.Reviews.FirstOrDefault(r => r.ReviewNumber == review.ReviewNumber);
        if (existingReview != null)
        {
            existingReview.Nnumber = review.Nnumber;
            existingReview.LetterQuality = review.LetterQuality;
            existingReview.ResumeQuality = review.ResumeQuality;
            existingReview.ResExpQuality = review.ResExpQuality;
            existingReview.ResumeComments = review.ResumeComments;
            existingReview.WritingSampleQuality = review.WritingSampleQuality;
            existingReview.WritingSampleComments = review.WritingSampleComments;
            existingReview.LORRelevance = review.LORRelevance;
            existingReview.LORQuality = review.LORQuality;
            existingReview.LORComments = review.LORComments;
            existingReview.OverallFitQuality = review.OverallFitQuality;
            existingReview.OverallFitComments = review.OverallFitComments;
            existingReview.DecisionRecommendation = review.DecisionRecommendation;
            existingReview.FollowUpRequired = review.FollowUpRequired;
            existingReview.FinalComments = review.FinalComments;
        }
    }

    public void DeleteReview(int reviewId)
    {
        var review = _context.Reviews.FirstOrDefault(r => r.ReviewNumber == reviewId);
        if (review != null)
        {
            _context.Reviews.Remove(review);
        }
    }
}
