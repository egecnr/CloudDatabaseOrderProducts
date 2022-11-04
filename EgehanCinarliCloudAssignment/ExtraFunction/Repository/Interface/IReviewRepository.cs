using ProductAndReviewFunction.DTO;
using ProductAndReviewFunction.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductAndReviewFunction.Repository.Interface
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetAllTheReviewByProductId(Guid productId);
        Task CreateReview(CreateReviewDTO createReviewDTO, Guid productId);
        Task<bool> CheckIfReviewExist(Guid productId);
        Task DeleteReview(Guid id);
        Task<Review> GetReviewById(Guid reviewId);
    }
}
