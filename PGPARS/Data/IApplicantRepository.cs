using PGPARS.Models;

namespace PGPARS.Data
{
    public interface IApplicantRepository
    {
        IEnumerable<Applicant> GetApplicants();
        Applicant GetApplicantById(int applicantId);

        
        //Applicant GetApplicantById(int id);
        void UpdateApplicant(Applicant applicant);
        //void DeleteApplicant(int id);
        //void AddApplicant(Applicant applicant);


    }
}
