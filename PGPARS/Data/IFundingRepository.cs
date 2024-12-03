using PGPARS.Models;
using System.Collections.Generic;

namespace PGPARS.Data
{
    public interface IFundingRepository
    {
        void AddFunding(Funding funding);
        void DeleteFunding(int id);
        IEnumerable<Funding> GetFunding();
        Funding GetFundingById(int fundingId);
        void UpdateFunding(Funding funding);
    }

}
