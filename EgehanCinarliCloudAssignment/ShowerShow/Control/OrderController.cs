using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System;
using User = UserAndOrdersFunction.Models.User;
using CreateUserDTO = UserAndOrdersFunction.DTO.CreateUserDTO;
using AutoMapper;
using UserAndOrdersFunction.Repository.Interface;
using UserAndOrdersFunction.DTO;
using UserAndOrdersFunction.Models;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using UserAndOrdersFunction.Utils;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using System.Security.Claims;
using UserAndOrdersFunction.Service;
using Microsoft.AspNetCore.Components.Forms;
using System.Text.Json;
using Azure.Storage.Queues;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace UserAndOrdersFunction.Controllers
{
    public class OrderController
    {
        private readonly ILogger<OrderController> _logger;
        private IOrderService orderService;

        public OrderController(ILogger<OrderController> log, IOrderService orderService)
        {
            _logger = log;
            this.orderService = orderService;
        }

        [Function("CreateOrder")]
        [OpenApiOperation(operationId: "CreateOrder", tags: new[] { "Orders " }, Summary = "Create a new order", Description = "This endpoint creates a new order")]
        [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiRequestBody("application/json", typeof(CreateOrderDTO), Description = "The order data.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(CreateUserDTO), Description = "The OK response with the new order.")]
        public async Task<HttpResponseData> CreateOrder([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "orders/{userId:Guid}/create")] HttpRequestData req, Guid userId)
        {
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                CreateOrderDTO orderDTO = JsonConvert.DeserializeObject<CreateOrderDTO>(requestBody);
                await orderService.CreateOrder(orderDTO, userId);
                responseData.StatusCode = HttpStatusCode.Created;
                responseData.Headers.Add("Result", "Order has been created");
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

