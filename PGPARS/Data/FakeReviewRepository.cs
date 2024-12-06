using PGPARS.Models;
using System.Collections.Generic;
namespace PGPARS.Data
{
    public class FakeReviewRepository : IReviewRepository
    {
        private List<Review> _reviewList = new List<Review>
        {
            new Review { LastName = "Buh" , FirstName = "Jimmy" , ReviewId = 1 , Status = "Accepted"}
        };
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
