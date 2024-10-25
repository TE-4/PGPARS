using PGPARS.Models;
using System.Collections.Generic;

namespace PGPARS.Data
{
    public class FakeFundingRepository : IFundingRepository
    {
        public IEnumerable<Funding> GetFunding()
        {
            return new List<Funding>
            {
                new Funding { Name = "Smitherson", Amount = 500 },
                new Funding { Name = "Peterson", Amount = 1000 },
                new Funding { Name = "Alicon", Amount = 300 },
                new Funding { Name = "Micicon", Amount = 700 },
                new Funding { Name = "Marshacon", Amount = 1500 }
            };
        }
    }
}
