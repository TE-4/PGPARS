using CsvHelper.Configuration;
using PGPARS.Models;

namespace PGPARS.Data
{
    public class ApplicantMap : ClassMap<Applicant>
    {
        public ApplicantMap() 
        {
            Map(m => m.Nnumber).Name("Slate Id");
            Map(m => m.FirstName).Name("First name");
            Map(m => m.LastName).Name("Last name");
            Map(m => m.email).Name("Email");
            Map(m => m.Phone).Name("Cell phone");
            Map(m => m.Status).Name("Application status");
            // Status
            // AppSubmitDate
            // MissingItems
            
        }

    }
}
