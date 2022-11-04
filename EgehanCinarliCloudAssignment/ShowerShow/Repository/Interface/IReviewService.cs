using ShowerShow.DTO;
using ShowerShow.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShowerShow.Repository.Interface
{
    public interface IReviewService
    {
        public Task AddReview(CreateReviewDTO createReviewDTO, Guid productId);
        public Task DeleteReview(Guid id);
        public Task<Review> GetReviewById(Guid reviewId);
        public Task<IEnumerable<Review>> GetReviewsForProduct(Guid productId);

    }
}