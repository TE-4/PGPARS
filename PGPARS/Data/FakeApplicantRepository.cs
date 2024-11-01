using PGPARS.Models;

namespace PGPARS.Data
{
    public class FakeApplicantRepository : IApplicantRepository
    {
        public IEnumerable<Applicant> GetApplicants()
        {
            return new List<Applicant>
        {
            new Applicant { Nnumber = "1", FirstName = "John", LastName = "Smith", Sex = "Male", Status = "Approved" },
            new Applicant { Nnumber = "2", FirstName = "Peter", LastName = "Student", Sex = "Male", Status = "Approved" },
            new Applicant { Nnumber = "3", FirstName = "Alice", LastName = "Johnson", Sex = "Female", Status = "Approved" },
            new Applicant { Nnumber = "4", FirstName = "Michael", LastName = "Davis", Sex = "Male", Status = "Denied" },
            new Applicant { Nnumber = "5", FirstName = "Marshall", LastName = "Mathers", Sex = "Male", Status = "Approved" },
            new Applicant { Nnumber = "6", FirstName = "Johnson", LastName = "Jigglers", Sex = "Male", Status = "Approved for funding" },
            new Applicant { Nnumber = "7", FirstName = "Tom", LastName = "Hanks", Sex = "Male", Status = "Approved for funding" }
        };
        }

        public Applicant GetApplicantById(int applicantId)
        {
            return GetApplicants().FirstOrDefault(a => a.Nnumber == applicantId.ToString());
        }
    }

}