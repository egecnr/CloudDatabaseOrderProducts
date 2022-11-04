using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ShowerShow.DTO;
using ShowerShow.Repository.Interface;

namespace ShowerShow.Control
{
    public class ProductController
    {
        private readonly ILogger _logger;
        private IProductService productService;
        public ProductController(ILoggerFactory loggerFactory,IProductService productService)
        {
            _logger = loggerFactory.CreateLogger<ProductController>();
            this.productService = productService;
        }

        [Function("CreateProduct")]
        [OpenApiOperation(operationId: "CreateProduct", tags: new[] { "Product " }, Summary = "Create a user account for online shopping", Description = "This endpoint creates a user account for online shopping")]
        [OpenApiRequestBody("application/json", typeof(ProductDTO), Description = "The user data.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(ProductDTO), Description = "The OK response with the new user.")]
        public async Task<HttpResponseData> CreateProduct([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "product/create")] HttpRequestData req)
        {
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                ProductDTO dto = JsonConvert.DeserializeObject<ProductDTO>(requestBody);
                await productService.CreateProduct(dto);
                responseData.StatusCode = HttpStatusCode.Created;
                responseData.Headers.Add("Result", "Product has been created");
            }
            catch (Exception e)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", e.Message);
            }
            return responseData;
        }
        /*[Function("GetUsersByName")]
        [OpenApiOperation(operationId: "GetUsersByName", tags: new[] { "Users " }, Summary = "Get users by username", Description = "This endpoint get users by username")]
        [OpenApiParameter(name: "userName", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The User ID parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<GetUserDTO>), Description = "The OK response with the new schedule.")]
        public async Task<HttpResponseData> GetUsersByName([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/{userName}")] HttpRequestData req, string userName)
        {
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                IEnumerable<GetUserDTO> users = await userService.GetUsersByName(userName);
                await responseData.WriteAsJsonAsync(users);
                responseData.StatusCode = HttpStatusCode.OK;
                return responseData;
            }
            catch (Exception e)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", e.Message);
            }
            return responseData;
        }
        [Function("GetUser")]
        [OpenApiOperation(operationId: "GetUserById", tags: new[] { "Users " }, Summary = "Get users by id", Description = "This endpoint get users by id")]
        [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The user ID parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(GetUserDTO), Description = "The OK response with the retrieved user")]
        public async Task<HttpResponseData> GetUser([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/{userId:Guid}")] HttpRequestData req, Guid userId)
        {
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                GetUserDTO userDTO = await userService.GetUserById(userId);
                await responseData.WriteAsJsonAsync(userDTO);
                responseData.StatusCode = HttpStatusCode.OK;
                return responseData;
            }
            catch (Exception e)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", e.Message);
            }
            return responseData;
        }*/
    }
}
