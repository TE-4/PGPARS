using PGPARS.Models;

namespace PGPARS.Data
{
    public class FakeApplicantRepository : IApplicantRepository
    {
        public IEnumerable<Applicant> GetApplicants()
        {
            return new List<Applicant>
            {
                new Applicant { Nnumber = "1", FirstName = "John Smith", Sex = "Male", Status = "Approved" },
                new Applicant { Nnumber = "2", FirstName = "Peter Student", Sex = "Male", Status = "Approved" },
                new Applicant { Nnumber = "3", FirstName = "Alice Johnson" , Sex = "Female", Status = "Approved"},   
                new Applicant { Nnumber = "4", FirstName = "Michael Davis" , Sex = "Male", Status = "Denied"},
                new Applicant { Nnumber = "5", FirstName = "Marshall Mathers" , Sex = "Male", Status = "Approved"}
            };
        }

        public void UpdateApplicant(Applicant applicant)
        {
            throw new NotImplementedException();
        }
    }
}
