using PGPARS.Models.ViewModels;
using PGPARS.Models;

namespace PGPARS.Data
{
        public interface IApplicantRepository
        {
            IEnumerable<ApplicantDisplayViewModel> GetFilteredApplicants();
            IEnumerable<Applicant> GetAllApplicants();
            Applicant GetApplicantById(int id);
        }
    }
