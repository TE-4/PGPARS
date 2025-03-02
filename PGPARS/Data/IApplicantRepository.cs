using PGPARS.Models;

namespace PGPARS.Data
{
    public interface IApplicantRepository
    {
        IEnumerable<Applicant> GetApplicants();
        Task<IEnumerable<Applicant>> GetApplicantsAsync();

        Applicant GetApplicantById(string Nnumber);

        int AddApplicants(List<Applicant> applicants);

        //Applicant GetApplicantById(int id);
        void UpdateApplicant(Applicant applicant);
        void DeleteApplicant(string Nnumber);
        //void AddApplicant(Applicant applicant);


    }
}
