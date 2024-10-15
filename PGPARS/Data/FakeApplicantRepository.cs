using PGPARS.Models;

namespace PGPARS.Data
{
    public class FakeApplicantRepository : IApplicantRepository
    {
        public IEnumerable<Applicant> GetApplicants()
        {
            return new List<Applicant>
            {
                new Applicant { Nnumber = "1", FirstName = "John Smith" },
                new Applicant { Nnumber = "2", FirstName = "Peter Student" },
                new Applicant { Nnumber = "3", FirstName = "Alice Johnson" },
                new Applicant { Nnumber = "4", FirstName = "Michael Davis" },
                new Applicant { Nnumber = "5", FirstName = "Marshall Mathers" }
            };
        }
    }
}
