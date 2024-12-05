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

        // New method for searching fundings
        IEnumerable<Funding> SearchFunding(string searchQuery);
    }

}
