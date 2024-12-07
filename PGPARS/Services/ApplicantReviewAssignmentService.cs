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

        public void AssignApplicantToReviewer(string nNumber, string reviewerId)
        {
            var applicant = _applicantRepository.GetApplicants().FirstOrDefault(a => a.Nnumber == nNumber);
            
        }

    }
}
