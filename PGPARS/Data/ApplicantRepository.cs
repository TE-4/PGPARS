using System.Globalization;
using System.IO;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
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

        public List<Applicant> GetApplicants()
        {
            return _context.Applicants.ToList();
        }

        public async Task AddApplicantAsync(Applicant applicant)
        {
            await _context.Applicants.AddAsync(applicant);
        }

        public async Task<List<Applicant>> GetApplicantsAsync()
        {
            return await _context.Applicants.ToListAsync(); 
        }

        public async Task<Applicant> GetApplicantByIdAsync(string Nnumber)
        {
            return await _context.Applicants.FindAsync(Nnumber); 
        }

        public void UpdateApplicant(Applicant applicant)
        {
            _context.Applicants.Update(applicant);
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
            return uploadCount;
        }

        public void DeleteApplicant(string Nnumber)
        {
            var applicant = _context.Applicants.FirstOrDefault(a => a.Nnumber == Nnumber); 
            if (applicant != null)
            {
                _context.Applicants.Remove(applicant);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
