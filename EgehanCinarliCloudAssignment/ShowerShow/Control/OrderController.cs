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
using UserAndOrdersFunction.Model;

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
        [Function("GetOrderByUserId")]
        [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
        [OpenApiOperation(operationId: "GetOrderByUserId", tags: new[] { "Order" }, Summary = "Get order by user id")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(GetOrderDTO))]
        public async Task<HttpResponseData> GetOrderByUserId([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "orders/user/{userId:Guid}")] HttpRequestData req, Guid userId)
        {
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                List<GetOrderDTO> order = (List<GetOrderDTO>)await orderService.GetOrdersOfUser(userId);
                await responseData.WriteAsJsonAsync(order);
                responseData.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", e.Message);
            }
            return responseData;
        }
        [Function("CreateOrder")]
        [OpenApiOperation(operationId: "CreateOrder", tags: new[] { "Order" }, Summary = "Create a new order to get products", Description = "Create a new order to get products")]
        [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiRequestBody("application/json", typeof(CreateOrderDTO), Description = "The order data.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(CreateOrderDTO), Description = "The OK response with the new order.")]
        public async Task<HttpResponseData> CreateOrder([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "orders/{userId:Guid}")] HttpRequestData req, Guid userId)
        {
            var responseData = req.CreateResponse();
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                CreateOrderDTO orderDTO = JsonConvert.DeserializeObject<CreateOrderDTO>(requestBody);
                await orderService.AddOrderToQueue(orderDTO, userId);
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
        [Function("GetOrderByOrderId")]
        [OpenApiParameter(name: "orderId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
        [OpenApiOperation(operationId: "GetOrderByOrderId", tags: new[] { "Order" }, Summary = "Get order by order id")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(GetOrderDTO))]
        public async Task<HttpResponseData> GetOrderByOrderId([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "orders/order/{orderId:Guid}")] HttpRequestData req, Guid orderId)
        {
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                GetOrderDTO order = await orderService.GetOrderByOrderId(orderId);
                await responseData.WriteAsJsonAsync(order);
                responseData.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", e.Message);
            }
            return responseData;
        }
        [Function("CreateProductInOrder")]
        [OpenApiOperation(operationId: "CreateProductInOrder", tags: new[] { "Order" }, Summary = "Create a new order")]
        [OpenApiParameter(name: "productId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
        [OpenApiParameter(name: "orderId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Order))]
        public async Task<HttpResponseData> CreateProductInOrder([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "orders/insertProduct/{orderId:Guid}/{productId:Guid}")] HttpRequestData req, Guid orderId, Guid productId)
        {
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                await orderService.CreateProductInOrder(orderId, productId);
                responseData.StatusCode = HttpStatusCode.OK;
                responseData.Headers.Add("Result", $"Product {productId} has been inserted in order list of {orderId}");
            }
            catch (Exception e)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", e.Message);
            }
            return responseData;
        }
        [Function("DeleteProductInOrder")]
        [OpenApiOperation(operationId: "DeleteProductInOrder", tags: new[] { "Order" }, Summary = "Delete a product in order")]
        [OpenApiParameter(name: "productId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
        [OpenApiParameter(name: "orderId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Order))]
        public async Task<HttpResponseData> DeleteProductInOrder([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "orders/deleteProduct/{orderId:Guid}/{productId:Guid}")] HttpRequestData req, Guid orderId, Guid productId)
        {
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                await orderService.DeleteProductInOrder(orderId, productId);
                responseData.StatusCode = HttpStatusCode.OK;
                responseData.Headers.Add("Result", $"Product {productId} has been deleted from order list of {orderId}");
            }
            catch (Exception e)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", e.Message);
            }
            return responseData;
        }
        [Function("CheckoutAndShipOrder")]
        [OpenApiOperation(operationId: "CheckoutAndShipOrder", tags: new[] { "Orders " }, Summary = "Check out and finalize the order. Also ship the order")]
        [OpenApiParameter(name: "orderId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Order))]
        public async Task<HttpResponseData> CheckoutAndShipOrder([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "orders/{orderId:Guid}/ship")] HttpRequestData req, Guid orderId)
        {
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                await orderService.CheckoutAndShipOrder(orderId);
                responseData.StatusCode = HttpStatusCode.OK;
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

