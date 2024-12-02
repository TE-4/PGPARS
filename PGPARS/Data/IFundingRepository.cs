using PGPARS.Models;
using System.Collections.Generic;

namespace PGPARS.Data
{
    public interface IFundingRepository
    {
        IEnumerable<Funding> GetFunding();
        Funding GetFundingById(int fundingId);
        void UpdateFunding(Funding funding);
        void AddFunding(Funding funding);
        void DeleteFunding(int fundingId);
    }

}
