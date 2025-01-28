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
        public Applicant GetApplicantById(String Nnumber)
        {
            return _context.Applicants.Find(Nnumber);
        }
        public int AddApplicants(List<Applicant> applicants)
        {
            int uploadCount = 0;
            foreach (var applicant in applicants)
            {
                if (!_context.Applicants.Any(a => a.Nnumber == applicant.Nnumber))
                {
                    _context.Applicants.Add(applicant);
                    uploadCount++;
                }            
            }
            _context.SaveChanges();
            return uploadCount;
        }

        public void DeleteApplicant(String Nnumber)
        {
            var applicant = _context.Applicants.Find(Nnumber);
            _context.Applicants.Remove(applicant);
            _context.SaveChanges();
        }

        public Applicant GetApplicantById(int applicantId)
        {
            throw new NotImplementedException();
        }

        public void DeleteApplicant(int id)
        {
            throw new NotImplementedException();
        }
    }

}

