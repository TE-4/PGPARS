using PGPARS.Models;
using System.Collections.Generic;
namespace PGPARS.Data
{
    public class FakeReviewRepository : IReviewRepository
    {
        public IEnumerable<Review> GetReviews()
        {
            return new List<Review>
            {
                new Review { ReviewId = "Smith", IsAccepted = false },
                new Review { ReviewId = "Chris", IsAccepted = true },
                new Review { ReviewId = "Andy", IsAccepted = false },
            };
        }
    }
}
