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


    // Update method for funding
    public void UpdateFunding(Funding funding)
    {
        var existingFunding = _context.Fundings.FirstOrDefault(f => f.Id == funding.Id);

        if (existingFunding != null)
        {
            // Update all properties (automatically handles new fields)
            _context.Entry(existingFunding).CurrentValues.SetValues(funding);
            existingFunding.DateModified = DateTime.UtcNow; // Update timestamp
            _context.SaveChanges();
        }
        else
        {
            throw new KeyNotFoundException($"Funding with ID {funding.Id} not found.");
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

        // Perform a case-insensitive search
        return _context.Fundings
            .Where(f => (f.Source != null && EF.Functions.Like(f.Source, $"%{searchQuery}%")) ||
                        (f.Cohort != null && EF.Functions.Like(f.Cohort, $"%{searchQuery}%")))
            .ToList();
    }

    public IEnumerable<FundingAllocation> GetFundingAllocations()
    {
        return _context.FundingAllocations          
            .Include(fa => fa.Nnumber)
            .ToList();
    }
    public void AddAllocation(FundingAllocation allocation)
    {
        var funding = _context.Fundings.FirstOrDefault(f => f.Id == allocation.FundingID);
        if (funding != null)
        {
            // Deduct the allocated amount
            funding.Remaining -= allocation.AllocatedAmount ?? 0;
            _context.Fundings.Update(funding); // Update the funding source
        }

        _context.FundingAllocations.Add(allocation); // Add the allocation
        _context.SaveChanges(); // Save changes
    }




}
