using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Threading.Tasks;
using System;
using ShowerShow.Repository.Interface;

namespace ShowerShow.Controllers
{
    public class BlobStorageController
    {
        private readonly ILogger<BlobStorageController> _logger;
        private IBlobStorageService blobStorageService;

        public BlobStorageController(ILogger<BlobStorageController> log, IBlobStorageService blobStorageService)
        {
            _logger = log;
            this.blobStorageService = blobStorageService;
        }


        [Function("UploadProfilePicture")] // USE POSTMAN TO TEST
        [OpenApiParameter(name: "UserId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiOperation(operationId: "UploadProfilePicture", tags: new[] { "Blob Storage" })]
        public async Task<HttpResponseData> UploadProfilePicture([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user/{UserId:Guid}/profile/uploadpic")] HttpRequestData req, Guid UserId)
        {
            _logger.LogInformation("Uploading to blob.");

            HttpResponseData responseData = req.CreateResponse();
            try
            {
                await blobStorageService.UploadProfilePicture(req.Body, UserId);
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
        [Function("GetProfilePictureOfUser")] // USE POSTMAN TO TEST
        [OpenApiParameter(name: "UserId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiOperation(operationId: "GetProfilePictureOfUser", tags: new[] { "Blob Storage" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "image/jpeg", bodyType: typeof(byte[]), Description = "The OK response with the profile picture.")]
        public async Task<HttpResponseData> GetProfilePictureOfUser([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/{UserId:Guid}/profile/getpic")] HttpRequestData req, Guid UserId, FunctionContext functionContext)
        {
            _logger.LogInformation("Uploading to blob.");

            HttpResponseData responseData = req.CreateResponse();
            try
            {
                responseData = await blobStorageService.GetProfilePictureOfUser(responseData, UserId);
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
            
        [Function("DeleteUserProfilePicture")]
        [OpenApiOperation(operationId: "DeleteUserProfilePicture", tags: new[] { "Blob Storage" })]
        [OpenApiParameter(name: "UserId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        public async Task<HttpResponseData> DeleteUserProfilePicture([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "user/{UserId:Guid}/profile/deletepic")] HttpRequestData req, Guid UserId, FunctionContext functionContext)
        {
            _logger.LogInformation("Deleting user profile picture.");
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                await blobStorageService.DeleteProfilePicture(UserId);
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