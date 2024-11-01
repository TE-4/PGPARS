using PGPARS.Data;
using PGPARS.Models;
using System.Collections.Generic;

namespace PGPARS.Data
{
    public class FakeFundingRepository : IFundingRepository
    {
        private List<Funding> _fundingList = new List<Funding>
    {
            new Funding { Name = "Smitherson", FundingID = 1, Amount = 500 },
            new Funding { Name = "Peterson", FundingID = 2, Amount = 1000 },
            new Funding { Name = "Alicon", FundingID = 3, Amount = 300 },
            new Funding { Name = "Micicon", FundingID = 4, Amount = 700 },
            new Funding { Name = "Marshacon", FundingID = 5, Amount = 1500 }
    };

        public IEnumerable<Funding> GetFunding()
        {
            return _fundingList;
        }

        public Funding GetFundingById(int fundingId)
        {
           
            return _fundingList.FirstOrDefault(f => f.FundingID == fundingId);
        }

        public void UpdateFunding(Funding funding)
        {
            var existingFunding = _fundingList.FirstOrDefault(f => f.FundingID == funding.FundingID);
            if (existingFunding != null)
            {
                existingFunding.Applicant = funding.Applicant;
                existingFunding.ApplicantId = funding.ApplicantId;
            }
        }
    }
}