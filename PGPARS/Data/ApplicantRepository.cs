using System.Globalization;
using System.IO;
using CsvHelper;
using PGPARS.Models;
using PGPARS.Models.ViewModels;

namespace PGPARS.Data
{
    public class ApplicantRepository : IApplicantRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicantRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Applicant> GetApplicants()
        {
            return _context.Applicants.ToList();
        }

        // will map the CSV data directly into the Applicant model.
        public void ImportApplicantsFromCsv(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<Applicant>().ToList();
                _context.Applicants.AddRange(records);
                _context.SaveChanges();
            }
            //AddRange adds the parsed CSV data into the database in bulk
        }
        //The smaller model for the list
        public IEnumerable<ApplicantDisplayViewModel> GetFilteredApplicants()
        {
            return _context.Applicants
                .Select(a => new ApplicantDisplayViewModel
                {
                    Id = a.Id,
                    FullName = a.FirstName + " " + a.LastName,
                    Gender = a.Sex,
                    ApprovedStatus = a.Status == "Approved"  
                })
                .ToList();
        }
        //Applicant GetApplicantById(int id);
        public IEnumerable<Applicant> GetAllApplicants()
        {
            return _context.Applicants.ToList();
        }

        public Applicant GetApplicantById(int id)
        {
            return _context.Applicants.FirstOrDefault(a => a.Id == id);
        }
        //void UpdateApplicant(Applicant applicant);
        //void DeleteApplicant(int id);
        //void AddApplicant(Applicant applicant);
    }
}
