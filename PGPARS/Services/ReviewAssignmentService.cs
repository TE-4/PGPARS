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

        // Convert to Async later possibly for optimization 
        

        // Assign reviewers to applicants
        public async Task AssignReviewersAsync()
        {
            // first get all applicants
            var applicants = await _applicantRepository.GetApplicantsAsync();

            // This will get all committee members and the admin user too
            var committeeReviewers = await _userManager.GetUsersInRoleAsync("Committee");
            var adminReviewers = await _userManager.GetUsersInRoleAsync("Admin");

            // concatenate the two lists 
            var reviewers = committeeReviewers.Concat(adminReviewers).ToList();

            // verify there are at least two reviewers
            if (reviewers.Count < 2)
            {
                throw new System.Exception("There must be at least two reviewers to assign");
            }

            // obtain the existing reviews from the review table
            var reviews = await _reviewRepository.GetReviewsAsync();

            // shuffle the reviewers to randomize the assignment
            var shuffledReviewers = reviewers.OrderBy(x => Guid.NewGuid()).ToList();
            int reviewerIndex = 0;
            int reviewCount = shuffledReviewers.Count;

            foreach (var applicant in applicants)
            {
                // skip if the applicant already has two reviews
                if (applicant.NumberOfReviews >= 2)
                {
                    continue;
                }

                while (applicant.NumberOfReviews < 2)
                {
                    // get the next reviewer
                    var reviewer = shuffledReviewers[reviewerIndex];

                    // Assign the reviewer and the applicant to a review
                    var review = new Review
                    {
                        Nnumber = applicant.Nnumber,
                        AppUserId = reviewer.Id
                    };

                    // add the review to the database
                    await _reviewRepository.AddReviewAsync(review);

                    // update the applicant's number of reviews
                    applicant.NumberOfReviews++;

                    // update the Last Assigned Review for AppUser
                    reviewer.LastAssignedReview = DateTime.Now;

                    // increment the reviewer index and use modulus operator to wrap around when needed 
                    reviewerIndex = (reviewerIndex + 1) % reviewCount;


                    // Save changes to the database in one call to reduce latency
                    await _reviewRepository.SaveChangesAsync();
                    await _applicantRepository.SaveChangesAsync();
                }
            }
                
            

        } // end method

        // Unassign all reviewers from all applicants
        public async Task UnassignReviewers()
        {
            
        }

        // Manually assign a single reviewer to an applicant
        public async Task AssignReviewer(string nnumber, string reviewerId)
        {

        }

        // Manually assign a range of reviews
        public async Task AssignRangeOfReviewers(string[] Nnumbers)
        {

        }
        
    }
}
