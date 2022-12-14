using ProductAndReviewFunction.DTO;
using ProductAndReviewFunction.Model;
using ProductAndReviewFunction.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductAndReviewFunction.Service
{
    public class ReviewServices : IReviewService
    {
        private IReviewRepository reviewRepository;
        private IProductService productService;
        public ReviewServices(IReviewRepository reviewRepository, IProductService productService)
        {
            this.reviewRepository = reviewRepository;
            this.productService = productService;
        }

        public async Task CreateReview(CreateReviewDTO createReviewDTO, Guid productId)
        {
           if(await productService.CheckIfProductExist(productId))
            {
                await reviewRepository.CreateReview(createReviewDTO, productId);
            }
            else
            {
                throw new Exception($"Product with product id {productId} doesn't exist.");
            }
        }

        public async Task DeleteReview(Guid id)
        {
            if(await CheckIfReviewExist(id))
            {
                await reviewRepository.DeleteReview(id);
            }
            else{
                throw new Exception($"Review with id {id} does not exist.");
            }
        }

        public async Task<Review> GetReviewById(Guid reviewId)
        {
            if (await CheckIfReviewExist(reviewId))
            {
                return await reviewRepository.GetReviewById(reviewId);

            }
            else
            {
                throw new Exception($"Review with id {reviewId} does not exist.");
            }
        }

        public async Task<IEnumerable<Review>> GetAllTheReviewByProductId(Guid productId)
        {
            if (await productService.CheckIfProductExist(productId))
            {
                return await reviewRepository.GetAllTheReviewByProductId(productId);
            }
            else
            {
                throw new Exception($"Product with product id {productId} doesn't exist.");
            }
        }

        public async Task<bool> CheckIfReviewExist(Guid productId)
        {
            return await reviewRepository.CheckIfReviewExist(productId);
        }
    }
}
