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
        [OpenApiOperation(operationId: "CreateReview", tags: new[] { "Review" }, Summary = "Create a user account for online shopping", Description = "This endpoint creates a user account for online shopping")]
        [OpenApiRequestBody("application/json", typeof(CreateReviewDTO), Description = "The user data.")]
        [OpenApiParameter(name: "productId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The product id parameter")]
        public async Task<HttpResponseData> CreateReview([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "product/create/{productId:Guid}")] HttpRequestData req, Guid productId)
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
        //Continue from here
        /*
                [Function("GetProductById")]
                [OpenApiOperation(operationId: "GetProductById", tags: new[] { "Product" }, Summary = "Get a product by its id", Description = "Get a product by its product id")]
                [OpenApiParameter(name: "productId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The product id parameter")]
                [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Model.Product), Description = "The OK response with the new product.")]
                public async Task<HttpResponseData> GetProductById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "product/get/{productId:Guid}")] HttpRequestData req, Guid productId)
                {
                    HttpResponseData responseData = req.CreateResponse();
                    try
                    {
                        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                        Product product = await productService.GetProductById(productId);
                        await responseData.WriteAsJsonAsync(product);
                        responseData.StatusCode = HttpStatusCode.OK;
                        responseData.Headers.Add("Result", $"Successfully retrieved the product called {product.Name}");

                    }
                    catch (Exception e)
                    {
                        responseData.StatusCode = HttpStatusCode.BadRequest;
                        responseData.Headers.Add("Reason", e.Message);
                    }
                    return responseData;
                }

                [Function("GetAllProducts")]
                [OpenApiOperation(operationId: "GetAllProducts", tags: new[] { "Product" }, Summary = "Get all products", Description = "Get all products")]
                [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Model.Product), Description = "The OK response with the new product.")]
                public async Task<HttpResponseData> GetAllProducts([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "product/get")] HttpRequestData req)
                {
                    HttpResponseData responseData = req.CreateResponse();
                    try
                    {
                        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                        IEnumerable<Product> products = await productService.GetAllProducts();
                        await responseData.WriteAsJsonAsync(products);
                        responseData.StatusCode = HttpStatusCode.OK;
                        responseData.Headers.Add("Result", $"Successfully retrieved the products");

                    }
                    catch (Exception e)
                    {
                        responseData.StatusCode = HttpStatusCode.BadRequest;
                        responseData.Headers.Add("Reason", e.Message);
                    }
                    return responseData;
                }
                [Function("RemoveProduct")]
                [OpenApiOperation(operationId: "RemoveProduct", tags: new[] { "Product" }, Summary = "Remove product by id", Description = "Remove product by id")]
                [OpenApiParameter(name: "productId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The product id parameter")]
                [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Product), Description = "The OK response with the deleted product")]
                public async Task<HttpResponseData> RemoveProduct([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "product/delete/{productId:Guid}")] HttpRequestData req, Guid productId)
                {
                    HttpResponseData responseData = req.CreateResponse();
                    try
                    {
                        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                        await productService.RemoveProduct(productId);
                        responseData.StatusCode = HttpStatusCode.OK;
                        responseData.Headers.Add("Result", $"Successfully removed the product");

                    }
                    catch (Exception e)
                    {
                        responseData.StatusCode = HttpStatusCode.BadRequest;
                        responseData.Headers.Add("Reason", e.Message);
                    }
                    return responseData;
                }
                [Function("UpdateProductById")]
                [OpenApiOperation(operationId: "UpdateProductById", tags: new[] { "Product" }, Summary = "Update product by id", Description = "Update product by id")]
                [OpenApiParameter(name: "productId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The product id parameter")]
                [OpenApiRequestBody("application/json", typeof(CreateUpdateProductDTO), Description = "The product data.")]
                [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Product))]
                public async Task<HttpResponseData> UpdateProductById([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "product/update/{productId:Guid}")] HttpRequestData req, Guid productId)
                {
                    HttpResponseData responseData = req.CreateResponse();
                    try
                    {
                        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                        CreateUpdateProductDTO dto = JsonConvert.DeserializeObject<CreateUpdateProductDTO>(requestBody);

                        await productService.UpdateProduct(productId, dto);
                        responseData.StatusCode = HttpStatusCode.OK;
                    }
                    catch (Exception e)
                    {
                        responseData.StatusCode = HttpStatusCode.BadRequest;
                        responseData.Headers.Add("Reason", e.Message);
                    }
                    return responseData;
                }
            }*/
    }
}
