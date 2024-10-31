using PGPARS.Models;
using System.Collections.Generic;

namespace PGPARS.Data
{
    public interface IReviewRepository
    {
        IEnumerable<Review> GetReviews();
    }
}
