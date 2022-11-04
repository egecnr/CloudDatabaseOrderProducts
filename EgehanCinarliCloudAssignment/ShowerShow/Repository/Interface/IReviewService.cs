using UserAndOrdersFunction.DTO;
using UserAndOrdersFunction.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserAndOrdersFunction.Repository.Interface
{
    public interface IReviewService
    {
        public Task AddReview(CreateReviewDTO createReviewDTO, Guid productId);
        public Task DeleteReview(Guid id);
        public Task<Review> GetReviewById(Guid reviewId);
        public Task<IEnumerable<Review>> GetReviewsForProduct(Guid productId);

    }
}