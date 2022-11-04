using ExtraFunction.DTO;
using ExtraFunction.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraFunction.Repository.Interface
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
