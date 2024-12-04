using Microsoft.AspNetCore.Mvc;
using PGPARS.Data;
using PGPARS.Models;
using System.Collections.Generic;
using System.Linq;

namespace PGPARS.Data
{
    public class FundingRepository : IFundingRepository
    {
        private readonly ApplicationDbContext _context;

        public FundingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Method to get all fundings
        public IEnumerable<Funding> GetFunding()
        {
            return _context.Fundings.ToList();
        }

        // Updated method to use FundingID
        public Funding GetFundingById(int fundingId)
        {
            return _context.Fundings.FirstOrDefault(f => f.FundingID == fundingId);
        }

        // Update method for funding
        public void UpdateFunding(Funding funding)
        {
            var existingFunding = _context.Fundings.FirstOrDefault(f => f.FundingID == funding.FundingID);
            if (existingFunding != null)
            {
                existingFunding.Applicant = funding.Applicant;
                existingFunding.ApplicantId = funding.ApplicantId;
                existingFunding.Amount = funding.Amount;
                existingFunding.Comment = funding.Comment;
                existingFunding.DateModified = DateTime.UtcNow;
                _context.SaveChanges();
            }
        }

        // Add method for funding
        public void AddFunding(Funding funding)
        {
            _context.Fundings.Add(funding);
            _context.SaveChanges(); // Save the new funding entry to the database
        }

        // Delete method for funding
        public void DeleteFunding(int fundingId)
        {
            var funding = _context.Fundings.FirstOrDefault(f => f.FundingID == fundingId);
            if (funding != null)
            {
                _context.Fundings.Remove(funding);
                _context.SaveChanges(); // Save the changes to remove the funding
            }
        }
    }
}
