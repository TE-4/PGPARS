using PGPARS.Models;
using System.Collections.Generic;

namespace PGPARS.Data
{
    public class FakeFundingRepository : IFundingRepository
    {
    private List<Funding> _fundingList = new List<Funding>
        {
             new Funding { Name = "Smitherson",FundingID = 1, Amount = 500, },
                new Funding { Name = "Peterson",FundingID = 2, Amount = 1000 },
                new Funding { Name = "Alicon",FundingID = 3, Amount = 300 },
                new Funding { Name = "Micicon",FundingID = 4, Amount = 700 },
                new Funding { Name = "Marshacon",FundingID = 5, Amount = 1500 }
            };
        }
    }
}
