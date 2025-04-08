using Microsoft.EntityFrameworkCore;
using PGPARS.Models;
using System.Collections.Generic;

namespace PGPARS.Data
{
    public interface IFundingRepository
    {
        IEnumerable<Funding> GetFunding();
        Funding GetFundingById(int id);
        void AddFunding(Funding funding);
        void UpdateFunding(Funding funding);
        void DeleteFunding(int id);
        IEnumerable<FundingAllocation> GetFundingAllocations();

        Task<Funding> GetFundingDetailsByIdAsync(int fundingId);
        IEnumerable<string> GetFundTypesStartingWith(string term);
        IEnumerable<string> GetSourcesStartingWith(string term);



        IEnumerable<Funding> SearchFunding(string searchQuery);
        void AddAllocation(FundingAllocation allocation);
        void DeleteAllocation(int id);
        FundingAllocation GetFundingAllocationById(int id);
        void UpdateAllocation(FundingAllocation allocation);
    }

}
