using Microsoft.EntityFrameworkCore;
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

            // Ensure Remaining is correctly recalculated
            var totalAllocated = existingFunding.FundingAllocations.Sum(a => a.AllocatedAmount);
            existingFunding.Remaining = existingFunding.Amount - totalAllocated;

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

        _context.FundingAllocations.Add(allocation); // Add the allocation
        _context.SaveChanges(); // Save changes
    }



    public void UpdateAllocation(FundingAllocation allocation)
    {
        _context.FundingAllocations.Update(allocation);
        _context.SaveChanges();
    }

    public void DeleteAllocation(int id)
    {
        var allocation = _context.FundingAllocations.Find(id);
        if (allocation != null)
        {
            _context.FundingAllocations.Remove(allocation);
            _context.SaveChanges();
        }
    }


}
