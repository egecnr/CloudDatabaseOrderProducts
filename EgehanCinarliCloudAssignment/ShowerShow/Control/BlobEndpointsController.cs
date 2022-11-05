using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Threading.Tasks;
using System;
using UserAndOrdersFunction.Repository.Interface;

namespace UserAndOrdersFunction.Controllers
{
    public class BlobEndpointsController
    {
        private readonly ILogger<BlobEndpointsController> _logger;
        private IBlobStorageService blobStorageService;
        //Testing with postman is ideal
        public BlobEndpointsController(ILogger<BlobEndpointsController> log, IBlobStorageService blobStorageService)
        {
            _logger = log;
            this.blobStorageService = blobStorageService;
        }


        [Function("CreateProductPictureInBlob")] 
        [OpenApiParameter(name: "productId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
        [OpenApiOperation(operationId: "CreateProductPictureInBlob", tags: new[] { "Blob" })]
        public async Task<HttpResponseData> CreateProductPictureInBlob([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "product/pictures/{productId:Guid}")] HttpRequestData req, Guid productId)
        {

            var responseData = req.CreateResponse();
            try
            {
                await blobStorageService.CreateProductPictureInBlob(req.Body, productId);
                responseData.StatusCode = HttpStatusCode.OK;
                return responseData;
            }
            catch (Exception ex)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", ex.Message);
                return responseData;
            }
        }
        [Function("GetProductPictureByProductId")] // USE POSTMAN TO TEST
        [OpenApiParameter(name: "productId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
        [OpenApiOperation(operationId: "GetProductPictureByProductId", tags: new[] { "Blob" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "image/jpeg", bodyType: typeof(byte[]), Description = "The OK response with the profile picture.")]
        public async Task<HttpResponseData> GetProductPictureByProductId([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "product/pictures/{productId:Guid}")] HttpRequestData req, Guid productId)
        {

            var responseData = req.CreateResponse();
            try
            {
                responseData = await blobStorageService.GetProductPictureByProductId(responseData, productId);
                responseData.StatusCode = HttpStatusCode.OK;
                return responseData;
            }
            catch (Exception ex)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", ex.Message);
                return responseData;
            }
        }
            
        [Function("DeleteProductPictureInBlob")]
        [OpenApiOperation(operationId: "DeleteProductPictureInBlob", tags: new[] { "Blob" })]
        [OpenApiParameter(name: "productId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
        public async Task<HttpResponseData> DeleteProductPictureInBlob([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "product/pictures/{productId:Guid}")] HttpRequestData req, Guid productId)
        {
            var responseData = req.CreateResponse();
            try
            {
                await blobStorageService.DeleteProductPictureInBlob(productId);
                responseData.StatusCode = HttpStatusCode.OK;
                return responseData;
            }
            catch (Exception ex)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", ex.Message);
                return responseData;
            }
        }
    }

}