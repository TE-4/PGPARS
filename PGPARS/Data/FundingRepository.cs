﻿using Microsoft.EntityFrameworkCore;
using PGPARS.Data;
using PGPARS.Models;

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

    // Method to get a specific funding by ID
    public Funding GetFundingById(int fundingId)
    {
        return _context.Fundings
            .Include(f => f.FundingAllocations) // Ensure allocations are loaded
            .FirstOrDefault(f => f.Id == fundingId);
    }
    public IEnumerable<string> GetFundTypesStartingWith(string term)
    {
        return _context.Fundings
            .Where(f => f.FundType != null && f.FundType.StartsWith(term))
            .Select(f => f.FundType)
            .Distinct()
            .ToList();
    }

    public IEnumerable<string> GetSourcesStartingWith(string term)
    {
        return _context.Fundings
            .Where(f => f.Source != null && f.Source.StartsWith(term))
            .Select(f => f.Source)
            .Distinct()
            .ToList();
    }

    public async Task<Funding> GetFundingDetailsByIdAsync(int fundingId)
    {
        // this query will include the allocations and applicants along with the funding details

        return await _context.Fundings
            .Include(f => f.FundingAllocations)
                .ThenInclude(fa => fa.Applicant)
            .FirstOrDefaultAsync(f => f.Id == fundingId);
    }


    // Update method for funding
    public void UpdateFunding(Funding funding)
    {
        var existingFunding = _context.Fundings
            .Include(f => f.FundingAllocations) // Include allocations
            .FirstOrDefault(f => f.Id == funding.Id);

        if (existingFunding != null)
        {
            existingFunding.Source = funding.Source;
            existingFunding.Amount = funding.Amount;

            // Ensure remaining amount is recalculated
            existingFunding.Remaining = existingFunding.Amount - existingFunding.FundingAllocations.Sum(a => a.AllocatedAmount);

            existingFunding.DateModified = DateTime.UtcNow;
            _context.SaveChanges();
        }
    }


    // Add method for funding
    public void AddFunding(Funding funding)
    {
        funding.Remaining = funding.Amount ?? 0;  // Initialize RemainingAmount based on Amount
        _context.Fundings.Add(funding);
        _context.SaveChanges();
    }


    // Delete method for funding
    public void DeleteFunding(int fundingId)
    {
        var funding = _context.Fundings.FirstOrDefault(f => f.Id == fundingId);
        if (funding != null)
        {
            // Delete related allocations first
            _context.FundingAllocations.RemoveRange(funding.FundingAllocations);
            _context.Fundings.Remove(funding);
            _context.SaveChanges();
        }
        else
        {
            throw new KeyNotFoundException($"Funding with ID {fundingId} not found.");
        }
    }

    // Search funding by query
    public IEnumerable<Funding> SearchFunding(string searchQuery)
    {
        if (string.IsNullOrEmpty(searchQuery))
            return _context.Fundings.ToList();

        bool isNumeric = int.TryParse(searchQuery, out int cohortNumber);

        var query = _context.Fundings.AsQueryable();

        if (!string.IsNullOrEmpty(searchQuery))
        {
            query = query.Where(f => f.Source != null && EF.Functions.Like(f.Source, $"%{searchQuery}%"));
        }

        if (isNumeric)
        {
            query = query.Where(f => f.Cohort == cohortNumber);
        }

        return query.ToList();
    }


    public IEnumerable<FundingAllocation> GetFundingAllocations()
    {
        return _context.FundingAllocations
            .Include(fa => fa.Funding)  
            .Include(fa => fa.Applicant)
            .ToList();
    }
    public FundingAllocation GetFundingAllocationById(int id)
    {
        return _context.FundingAllocations
            .Include(fa => fa.Applicant) // Ensure Applicant is included if needed
             .Include(a => a.Funding)

            .FirstOrDefault(fa => fa.Id == id);
    }
    public void AddAllocation(FundingAllocation allocation)
    {
        var funding = _context.Fundings.FirstOrDefault(f => f.Id == allocation.FundingID);
        if (funding != null)
        {
            // Deduct the allocated amount
            funding.Remaining -= allocation.AllocatedAmount;
            _context.Fundings.Update(funding); // Update the funding source
        }
        _context.FundingAllocations.Add(allocation);
        _context.SaveChanges();
    }



    public void UpdateAllocation(FundingAllocation allocation)
    {
        var existingAllocation = _context.FundingAllocations
            .Include(fa => fa.Applicant) // Include Applicant
            .FirstOrDefault(fa => fa.Id == allocation.Id);

        if (existingAllocation != null)
        {
            var funding = _context.Fundings
                .Include(f => f.FundingAllocations)
                    .ThenInclude(fa => fa.Applicant) // Ensure Applicants are included
                .FirstOrDefault(f => f.Id == allocation.FundingID);

            if (funding != null)
            {
                existingAllocation.AllocatedAmount = allocation.AllocatedAmount;

                // Recalculate remaining funding amount
                funding.Remaining = funding.Amount - funding.FundingAllocations.Sum(a => a.AllocatedAmount);

                // Save changes
                _context.FundingAllocations.Update(existingAllocation);
                _context.SaveChanges();

                _context.Fundings.Update(funding);
                _context.SaveChanges();
            }
        }
    }



    public void DeleteAllocation(int id)
    {
        var allocation = _context.FundingAllocations.Find(id);
        if (allocation != null)
        {
            // Get the associated funding
            var funding = _context.Fundings.Include(f => f.FundingAllocations)
                                           .FirstOrDefault(f => f.Id == allocation.FundingID);
            if (funding != null)
            {
                // Recalculate the remaining amount before deleting the allocation
                funding.Remaining = funding.Amount - funding.FundingAllocations
                    .Where(a => a.Id != id) // Exclude the allocation being deleted
                    .Sum(a => a.AllocatedAmount);

                // Update the funding first
                _context.Fundings.Update(funding);
                _context.SaveChanges(); // Save funding update

                // Remove the allocation
                _context.FundingAllocations.Remove(allocation);
                _context.SaveChanges(); // Save allocation deletion
            }
        }
    }




}
