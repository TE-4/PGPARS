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
        public Applicant GetApplicantById(int applicantId)
        {
            return _context.Applicants.Find(applicantId);
        }

    }

}

