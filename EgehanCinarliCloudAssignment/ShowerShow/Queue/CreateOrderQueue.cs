using System;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using UserAndOrdersFunction.DTO;
using UserAndOrdersFunction.Model;
using UserAndOrdersFunction.Repository.Interface;

namespace UserAndOrdersFunction.Queue
{
    public class CreateOrderQueue
    {
        private readonly ILogger _logger;
        private IOrderService orderService;

        public CreateOrderQueue(ILoggerFactory loggerFactory,IOrderService orderService)
        {
            _logger = loggerFactory.CreateLogger<CreateOrderQueue>();
            this.orderService = orderService;
        }

        [Function("CreateOrderQueue")]
        public void Run([QueueTrigger("create-order-queue", Connection = "AzureWebJobsStorage")] string myQueueItem)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
            Order order = JsonSerializer.Deserialize<Order>(myQueueItem);
            orderService.CreateOrder(order);
        }
    }
}
