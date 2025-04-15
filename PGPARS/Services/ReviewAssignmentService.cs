using Microsoft.AspNetCore.Identity;
using PGPARS.Data;
using PGPARS.Models;
using System.Linq;

namespace PGPARS.Services
{
    public class ReviewAssignmentService
    {
        private readonly IApplicantRepository _applicantRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IReviewRepository _reviewRepository;

        public ReviewAssignmentService(IApplicantRepository applicantRepository, UserManager<AppUser> userManager, IReviewRepository reviewRepository)
        {
            _applicantRepository = applicantRepository;
            _userManager = userManager;
            _reviewRepository = reviewRepository;
        }

        
        // Assign reviewers to applicants
        public async Task AssignReviewersAsync(List<string>? selectedApplicants)
        {

            // Create a list of applicants to be assigned
            List<Applicant> applicants = new List<Applicant>();

            // Either get all applicants, or just use the input list of applicants
            if (selectedApplicants != null && selectedApplicants.Count > 0)
            {
                applicants = await _applicantRepository.GetApplicantsByNnumbersAsync(selectedApplicants);
            }
            else
            {
                applicants = await _applicantRepository.GetApplicantsAsync();
            }
                

            var committeeReviewers = await _userManager.GetUsersInRoleAsync("Committee");
            var adminReviewers = await _userManager.GetUsersInRoleAsync("Admin");

            var reviewers = committeeReviewers.Concat(adminReviewers).ToList();

            // ensures there are at least two committee members/admin
            if (reviewers.Count < 2)
            {
                throw new Exception("There must be at least two reviewers to assign");
            }

            // shuffle the reviewers
            var shuffledReviewers = reviewers.OrderBy(x => Guid.NewGuid()).ToList();
            int reviewerIndex = 0;
            int reviewCount = shuffledReviewers.Count;

            // this list is to save in memory and only save to the database once at the end 
            List<Review> newReviews = new List<Review>();

            foreach (var applicant in applicants)
            {
                // skip applicants that already have 2 reviews
                if (applicant.NumberOfReviews >= 2) continue;

                while (applicant.NumberOfReviews < 2)
                {
                    var reviewer = shuffledReviewers[reviewerIndex];

                    var review = new Review
                    {
                        Nnumber = applicant.Nnumber,
                        AppUserId = reviewer.Id,
                        ReviewDate = DateTime.Now,
                        Cohort = applicant.Cohort ?? 0
                    };

                    newReviews.Add(review);

                    // Update the applicant and reviewer
                    applicant.NumberOfReviews++; 
                    reviewer.LastAssignedReview = DateTime.Now;

                    reviewerIndex = (reviewerIndex + 1) % reviewCount; 
                }
            }

            // Save all changes
            if (newReviews.Count > 0)
            {
                await _reviewRepository.AddReviewsAsync(newReviews);
                await _reviewRepository.SaveChangesAsync();
                await _applicantRepository.SaveChangesAsync();
            }
        }

        // Unassign all incomplete reviews
        public async Task UnassignIncompleteReviewsAsync()
        {
            // get all incomplete reviews
            var incompleteReviews = await _reviewRepository.GetReviewsAsync();
            var reviewsToDelete = incompleteReviews.Where(r => !r.ReviewComplete).ToList();

            if (!reviewsToDelete.Any())
            {
                throw new InvalidOperationException("No incomplete reviews found to unassign.");
            }

            // get affected applicants
            var affectedApplicantIds = reviewsToDelete.Select(r => r.Nnumber).Distinct().ToList();
            var affectedApplicants = await _applicantRepository.GetApplicantsByNnumbersAsync(affectedApplicantIds);

            // reset the review count for affected applicants
            affectedApplicants.ForEach(a => a.NumberOfReviews = 0);
           

            // delete all incomplete reviews
            foreach (var review in reviewsToDelete)
            {
                _reviewRepository.DeleteReview(review.ReviewNumber);
            }

            // save all changes at the end 
            await _reviewRepository.SaveChangesAsync();
            await _applicantRepository.SaveChangesAsync();
        }


        // Manually assign a single reviewer to an applicant
        public async Task AssignReviewer(string nnumber, string reviewerId)
        {
            // get the applicant first by nnumber
            var applicant = await _applicantRepository.GetApplicantByIdAsync(nnumber);
            if (applicant == null)
            {
                throw new InvalidOperationException("Applicant not found.");
            }

            // make sure applicant doesnt already have 2 reviews
            if (applicant.NumberOfReviews >= 2)
            {
                throw new InvalidOperationException("This applicant already has two assigned reviews.");
            }

            // get the reviewer based on the input parameter reviewerId
            var reviewer = await _userManager.FindByIdAsync(reviewerId);
            if (reviewer == null)
            {
                throw new InvalidOperationException("Reviewer not found.");
            }

            // check if this reviewer has already been assigned to this applicant
            var existingReviews = await _reviewRepository.GetReviewsAsync();
            if (existingReviews.Any(r => r.Nnumber == nnumber && r.AppUserId == reviewerId))
            {
                throw new InvalidOperationException("This reviewer has already been assigned to this applicant.");
            }

            // create and assign the review
            var review = new Review
            {
                Nnumber = nnumber,
                AppUserId = reviewer.Id,
                ReviewDate = DateTime.Now,
                Cohort = applicant.Cohort ?? 0
            };

            await _reviewRepository.AddReviewAsync(review);

            // update appster
            applicant.NumberOfReviews++;

            // save all changes to database
            await _reviewRepository.SaveChangesAsync();
            await _applicantRepository.SaveChangesAsync();
        }


        
        
    }
}
