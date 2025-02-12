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
        private readonly ApplicationDbContext _context;

        public ReviewAssignmentService(IApplicantRepository applicantRepository, UserManager<AppUser> userManager, ApplicationDbContext context)
        {
            _applicantRepository = applicantRepository;
            _userManager = userManager;
            _context = context;
        }

        // Assign reviewers to applicants
        public async Task AssignReviewers()
        {
            var applicants = _applicantRepository.GetApplicants();
            var reviewers = await _userManager.GetUsersInRoleAsync("Committee");

            if (reviewers.Count == 0)
            {
                throw new Exception("No reviewers available.");
            }

            int limit = reviewers.Count;
            int count = 0;

            foreach (var applicant in applicants)
            {
                // Check if applicant already has 2 reviewers
                var existingReviewers = _context.ApplicantReviewers
                    .Where(ar => ar.Nnumber == applicant.Nnumber)
                    .ToList();

                if (existingReviewers.Count >= 2)
                {
                    continue; // Skip if already assigned two reviewers
                }

                bool updated = false;

                while (existingReviewers.Count < 2)
                {
                    if (count >= limit)
                    {
                        count = 0;
                    }

                    var reviewer = reviewers[count];

                    // Ensure the same reviewer is not assigned twice
                    if (!existingReviewers.Any(ar => ar.AppUserId == reviewer.Id))
                    {
                        var applicantReviewer = new ApplicantReviewer
                        {
                            Nnumber = applicant.Nnumber,
                            AppUserId = reviewer.Id
                        };

                        _context.ApplicantReviewers.Add(applicantReviewer);
                        _context.SaveChanges(); // Persist changes
                        existingReviewers.Add(applicantReviewer);
                        updated = true;
                    }

                    count++;
                }

                if (updated)
                {
                    _applicantRepository.UpdateApplicant(applicant);
                }
            }
        }

        // Unassign all reviewers from all applicants
        public async Task UnassignReviewers()
        {
            var allAssignments = _context.ApplicantReviewers.ToList();
            _context.ApplicantReviewers.RemoveRange(allAssignments);
            await _context.SaveChangesAsync();
        }

        // Manually assign a reviewer to an applicant
        public async Task AssignReviewerAsync(string nnumber, string reviewerId)
        {
            var applicant = _applicantRepository.GetApplicantById(nnumber);
            var reviewer = await _userManager.FindByIdAsync(reviewerId);

            if (applicant == null)
            {
                throw new Exception("Applicant not found.");
            }

            if (reviewer == null)
            {
                throw new Exception("Reviewer not found.");
            }

            // Check if reviewer is already assigned
            bool alreadyAssigned = _context.ApplicantReviewers
                .Any(ar => ar.Nnumber == applicant.Nnumber && ar.AppUserId == reviewer.Id);

            if (alreadyAssigned)
            {
                throw new Exception("Reviewer is already assigned to this applicant.");
            }

            var applicantReviewer = new ApplicantReviewer
            {
                Nnumber = applicant.Nnumber,
                AppUserId = reviewer.Id
            };

            _context.ApplicantReviewers.Add(applicantReviewer);
            await _context.SaveChangesAsync();
        }
    }
}
