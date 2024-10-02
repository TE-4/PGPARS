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
                //new Applicant { Id = 2, FullName = "Peter Student" },
                //new Applicant { Id = 3, FullName = "Alice Johnson" },
                //new Applicant { Id = 4, FullName = "Michael Davis" },
                //new Applicant { Id = 5, FullName = "Marshall Mathers" }
            };
        }
    }
}
