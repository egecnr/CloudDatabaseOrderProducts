using ShowerShow.DTO;
using ShowerShow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Repository.Interface
{
    public interface IReviewRepository
    {
        public Task CreateReview(CreateReviewDTO createReviewDTO, Guid productId);
        public Task DeleteReview(Guid id);
        public Task<Review> GetReviewById(Guid reviewId);
        public Task<IEnumerable<Review>> GetReviewsForProduct(Guid productId);


    }
}
