using PGPARS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PGPARS.Data
{
    public interface IApplicantRepository
    {
        List<Applicant> GetApplicants(); 

        Task<List<Applicant>> GetApplicantsAsync(); 

        Task<Applicant> GetApplicantByIdAsync(string Nnumber); 

        int AddApplicants(List<Applicant> applicants);

        void UpdateApplicant(Applicant applicant);

        void DeleteApplicant(string Nnumber);

        Task SaveChangesAsync();
    }
}
