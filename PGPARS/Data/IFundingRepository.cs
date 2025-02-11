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
        IEnumerable<FundingAllocations> GetFundingAllocations();


       
        IEnumerable<Funding> SearchFunding(string searchQuery);
        void AddAllocation(FundingAllocations allocation);
    }

}
