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

        // Look into if these should be async or not (thinking yes)
        

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

            // retrieve the reviews 
            var reviews = _reviewRepository.GetReviews();

            // loop through the applicants and assign reviewers
            foreach ( var applicant in applicants)
            {
               
            }

        }

        // Unassign all reviewers from all applicants
        public async Task UnassignReviewers()
        {
            
        }

        // Manually assign a reviewer to an applicant
        public async Task AssignReviewer(string nnumber, string reviewerId)
        {

        }

        public async Task AssignRangeOfReviewers(string[] Nnumbers)
        {

        }
        
    }
}
