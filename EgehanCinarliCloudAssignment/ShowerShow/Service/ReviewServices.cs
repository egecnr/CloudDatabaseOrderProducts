using AutoMapper;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using ShowerShow.DTO;
using ShowerShow.Model;
using ShowerShow.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShowerShow.Service
{
    public class ReviewServices : IReviewService
    {
        private IReviewRepository reviewRepository;

        public ReviewServices(IReviewRepository reviewRepository)
        {
            this.reviewRepository = reviewRepository; 
        }

        public async Task AddReview(CreateReviewDTO createReviewDTO, Guid productId)
        {
         await reviewRepository.CreateReview(createReviewDTO, productId);
        }

        public async Task DeleteReview(Guid id)
        {
            await reviewRepository.DeleteReview(id);
        }

        public async Task<Review> GetReviewById(Guid reviewId)
        {
            return await reviewRepository.GetReviewById(reviewId);
        }

        public async Task<IEnumerable<Review>> GetReviewsForProduct(Guid productId)
        {
            return await reviewRepository.GetReviewsForProduct(productId);
        }
    }
}
