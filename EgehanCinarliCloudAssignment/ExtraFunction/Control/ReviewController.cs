using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using ProductAndReviewFunction.Model;
using ProductAndReviewFunction.Repository.Interface;
using ProductAndReviewFunction.DTO;
using System.Collections;
using System.Collections.Generic;

namespace ProductAndReviewFunction.Control
{
    public class ReviewController
    {
        private readonly ILogger _logger;
        private IReviewService reviewService;

        public ReviewController(ILoggerFactory loggerFactory, IReviewService reviewService)
        {
            _logger = loggerFactory.CreateLogger<ReviewController>();
            this.reviewService = reviewService;
        }

        [Function("CreateReview")]
        [OpenApiOperation(operationId: "CreateReview", tags: new[] { "Review" }, Summary = "Create a review", Description = "This endpoint creates a review for a product")]
        [OpenApiRequestBody("application/json", typeof(CreateReviewDTO), Description = "The user data.")]
        [OpenApiParameter(name: "productId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The product id parameter")]
        public async Task<HttpResponseData> CreateReview([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "review/create/{productId:Guid}")] HttpRequestData req, Guid productId)
        {
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                CreateReviewDTO dto = JsonConvert.DeserializeObject<CreateReviewDTO>(requestBody);
                await reviewService.CreateReview(dto, productId);
                responseData.StatusCode = HttpStatusCode.Created;
                responseData.Headers.Add("Result", $"Review has been created has been created for the product with id {productId}");
            }
            catch (Exception e)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", e.Message);
            }
            return responseData;
        }


        [Function("GetReviewById")]
        [OpenApiOperation(operationId: "GetReviewById", tags: new[] { "Review" }, Summary = "Get a review by its id", Description = "Get a rebiew by its review id")]
        [OpenApiParameter(name: "reviewId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The review id parameter")]
        public async Task<HttpResponseData> GetReviewById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "review/{reviewId:Guid}")] HttpRequestData req, Guid reviewId)
        {
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                Review review = await reviewService.GetReviewById(reviewId);
                await responseData.WriteAsJsonAsync(review);
                responseData.StatusCode = HttpStatusCode.OK;
                responseData.Headers.Add("Result", $"Successfully retrieved the review with the id {review.Id}");

            }
            catch (Exception e)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", e.Message);
            }
            return responseData;
        }

        [Function("GetAllReviewsByProductId")]
        [OpenApiOperation(operationId: "GetAllReviewsByProductId", tags: new[] { "Review" }, Summary = "Get all reviews of a product", Description = "Get all reviews of one product to see everyone's opinion")]
        [OpenApiParameter(name: "productId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The product id parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Review), Description = "The OK response with the new product.")]
        public async Task<HttpResponseData> GetAllReviewsByProductId([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "review/product/{productId:Guid}")] HttpRequestData req,Guid productId)
        {
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                IEnumerable<Review> reviews = await reviewService.GetAllTheReviewByProductId(productId);
                await responseData.WriteAsJsonAsync(reviews);
                responseData.StatusCode = HttpStatusCode.OK;
                responseData.Headers.Add("Result", $"Successfully retrieved the reviews for product {productId}");

            }
            catch (Exception e)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", e.Message);
            }
            return responseData;
        }
        [Function("DeleteReview")]
        [OpenApiOperation(operationId: "DeleteReview", tags: new[] { "Review" }, Summary = "Remove review by id", Description = "Remove review by id")]
        [OpenApiParameter(name: "reviewId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The review id parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Product), Description = "The OK response with the deleted product")]
        public async Task<HttpResponseData> DeleteReview([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "review/delete/{reviewId:Guid}")] HttpRequestData req, Guid reviewId)
        {
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                await reviewService.DeleteReview(reviewId);
                responseData.StatusCode = HttpStatusCode.OK;
                responseData.Headers.Add("Result", $"Successfully deleted the review");

            }
            catch (Exception e)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", e.Message);
            }
            return responseData;
        }
        
    }
}

