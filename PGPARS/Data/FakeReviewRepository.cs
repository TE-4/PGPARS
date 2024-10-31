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
                new Review { ReviewId = "Smith", IsRejected = false },
                new Review { ReviewId = "Chris", IsRejected = true },
                new Review { ReviewId = "Andy", IsRejected = false },
            };
        }
    }
}
