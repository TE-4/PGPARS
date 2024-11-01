using System.Globalization;
using System.IO;
using CsvHelper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        public void UpdateApplicant(Applicant applicant)
        {
            _context.Applicants.Update(applicant);
            _context.SaveChanges();
        }

        // will map the CSV data directly into the Applicant model.
        public void ImportApplicantsFromCsv(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                // Read the header
                csv.Read();
                csv.ReadHeader(); // This will read the header and allow you to skip it

                // Get records while skipping the header
                var records = csv.GetRecords<Applicant>().ToList();

                foreach (var record in records)
                {
                    var existingApplicant = _context.Applicants
                        .SingleOrDefault(a => a.Nnumber == record.Nnumber);

                    if (existingApplicant != null)
                    {
                        // Update existing record
                        _context.Entry(existingApplicant).CurrentValues.SetValues(record);
                    }
                    else
                    {
                        // Add new record
                        _context.Applicants.Add(record);
                    }
                }

                _context.SaveChanges();
            }
        }
    }
    //Applicant GetApplicantById(int id);
    //void DeleteApplicant(int id);
    //void AddApplicant(Applicant applicant);
}

