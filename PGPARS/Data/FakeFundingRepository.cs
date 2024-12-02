using PGPARS.Data;
using PGPARS.Models;
using System.Collections.Generic;
using System.Linq;

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

        // Updated method to use FundingID
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
        public void AddFunding(Funding funding)
        {
            funding.FundingID = _fundingList.Max(f => f.FundingID) + 1; // Generate a new ID
            _fundingList.Add(funding);
        }

        public void DeleteFunding(int fundingId)
        {
            var funding = _fundingList.FirstOrDefault(f => f.FundingID == fundingId);
            if (funding != null)
            {
                _fundingList.Remove(funding);
            }
        }


    }
}
