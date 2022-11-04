using AutoMapper;
using ProductAndReviewFunction.DAL;
using ProductAndReviewFunction.DTO;
using ProductAndReviewFunction.Model;
using ProductAndReviewFunction.Repository.Interface;
using ProductAndReviewFunction.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProductAndReviewFunction.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private DatabaseContext dbContext;


        public ReviewRepository(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<bool> CheckIfReviewExist(Guid productId)
        {
            await dbContext.SaveChangesAsync();
            if (dbContext.ProductReviews.Count(x => x.Id == productId) > 0)
                return true;
            else
                return false;
        }

        public async Task CreateReview(CreateReviewDTO createReviewDTO, Guid productId)
        {
            Mapper mapper = AutoMapperUtil.ReturnMapper(new MapperConfiguration(con => con.CreateMap<CreateReviewDTO, Review>()));
            Review review = mapper.Map<Review>(createReviewDTO);
            review.ProductId = productId;
            await dbContext.ProductReviews.AddAsync(review);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteReview(Guid id)
        {
            Review review = await GetReviewById(id);
            dbContext.ProductReviews.Remove(review);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Review> GetReviewById(Guid reviewId)
        {
            await dbContext.SaveChangesAsync();
            return dbContext.ProductReviews.FirstOrDefault(c => c.Id == reviewId);
        }

        public async Task<IEnumerable<Review>> GetAllTheReviewByProductId(Guid productId)
        {
            await dbContext.SaveChangesAsync();
            return dbContext.ProductReviews.Where(c => c.ProductId == productId);
        }
    }
}
