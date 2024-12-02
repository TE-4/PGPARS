using CsvHelper.Configuration;
using PGPARS.Models;

namespace PGPARS.Data
{
    public class FacultyMap : ClassMap<AppUser>
    {
        public FacultyMap() 
        { 
            Map(m => m.Nnumber).Name("Nnumber");
            Map(m => m.FirstName).Name("FirstName");
            Map(m => m.LastName).Name("LastName");
            Map(m => m.Email).Name("email");
            Map(m => m.Position).Name("Position");
        }
    }
}
