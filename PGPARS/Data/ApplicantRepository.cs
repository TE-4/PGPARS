using System.Globalization;
using System.IO;
using CsvHelper;
using PGPARS.Models;
using PGPARS.Models.ViewModels;

namespace PGPARS.Data
{
    public class ApplicantRepository
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
            //AddRange adds the parsed CSV data into the database in bulk
        }
   

        }
        //Applicant GetApplicantById(int id);

        //void UpdateApplicant(Applicant applicant);
        //void DeleteApplicant(int id);
        //void AddApplicant(Applicant applicant);
    }

