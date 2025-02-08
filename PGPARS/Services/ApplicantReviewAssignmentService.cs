using Microsoft.AspNetCore.Identity;
using PGPARS.Data;
using PGPARS.Models;

namespace PGPARS.Services
{
    public class ApplicantReviewAssignmentService
    {
        private readonly IApplicantRepository _applicantRepository;
        private readonly UserManager<AppUser> _userManager;

        public ApplicantReviewAssignmentService(IApplicantRepository applicantRepository, UserManager<AppUser> userManager)
        {
            _applicantRepository = applicantRepository;
            _userManager = userManager;
        }

        // assign reviewers to applicants
        public async Task AssignReviewers()
        {
            var applicants = _applicantRepository.GetApplicants(); // add await for async query?
            var reviewers = await _userManager.GetUsersInRoleAsync("Committee");
            int limit = reviewers.Count;
            int count = 0;
            foreach (var applicant in applicants)
            {
                bool updated = false;

                if (applicant.Reviewer1 == null)
                {
                    if (count >= limit)
                    {
                        count = 0;
                    }
                    applicant.Reviewer1 = reviewers[count].ShortName;
                    count++;
                    updated = true;
                }
                if (applicant.Reviewer2 == null)
                {
                    if (count >= limit)
                    {
                        count = 0;
                    }
                    applicant.Reviewer2 = reviewers[count].ShortName;
                    count++;
                    updated = true;
                }
                if (updated)
                {
                    _applicantRepository.UpdateApplicant(applicant);
                }
            }
        } // end method

        // if needed, unassign all reviewers from all applicants
        public async Task UnassignReviewers()
        {
            var applicants = _applicantRepository.GetApplicants();
            foreach (var applicant in applicants)
            {
                applicant.Reviewer1 = null;
                applicant.Reviewer2 = null;
                _applicantRepository.UpdateApplicant(applicant);
            }
        } // end method

        // manually assign a reviewer to an applicant
        public async Task AssignReviewerAsync(string applicantId, string reviewerNnumber)
        {
            var applicant = _applicantRepository.GetApplicantById(applicantId);
            var reviewer = await _userManager.FindByIdAsync(reviewerNnumber);

            if (reviewer == null)
            {
                throw new Exception("Reviewer not found.");
            }

            if(applicant.Reviewer1 != null)
            {
                
            }

            // Assign the reviewer to the applicant here
        }


    } // end class
} // end namespace
