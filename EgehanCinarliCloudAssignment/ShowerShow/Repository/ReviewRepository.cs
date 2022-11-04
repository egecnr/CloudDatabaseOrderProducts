using AutoMapper;
using Azure.Storage.Queues;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using ShowerShow.DAL;
using ShowerShow.DTO;
using ShowerShow.Model;
using ShowerShow.Models;
using ShowerShow.Repository.Interface;
using ShowerShow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShowerShow.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private DatabaseContext dbContext;


        public ReviewRepository(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task CreateReview(CreateReviewDTO createReviewDTO, Guid productId)
        {
            Mapper mapper = AutoMapperUtil.ReturnMapper(new MapperConfiguration(con => con.CreateMap<CreateReviewDTO, Review>()));
            Review productReview = mapper.Map<Review>(createReviewDTO);
            productReview.ProductId = productId;
            await dbContext.ProductReviews.AddAsync(productReview);
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

        public async Task<IEnumerable<Review>> GetReviewsForProduct(Guid productId)
        {
            await dbContext.SaveChangesAsync();
            return dbContext.ProductReviews.Where(c => c.ProductId == productId);
        }
    }
}
