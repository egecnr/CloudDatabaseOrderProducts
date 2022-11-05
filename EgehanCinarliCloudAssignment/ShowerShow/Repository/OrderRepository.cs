using AutoMapper;
using Azure.Storage.Queues;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using UserAndOrdersFunction.DAL;
using UserAndOrdersFunction.DTO;
using UserAndOrdersFunction;
using UserAndOrdersFunction.Models;
using UserAndOrdersFunction.Repository.Interface;
using UserAndOrdersFunction.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UserAndOrdersFunction.Model;

namespace UserAndOrdersFunction.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private DatabaseContext dbContext;
        private IProductService productService;


        public OrderRepository(DatabaseContext dbContext, IProductService productService)
        {
            this.dbContext = dbContext;
            this.productService = productService;
        }

        public async Task CreateProductInOrder(Guid orderId, Guid productId)
        {
            Order order = dbContext.Orders.FirstOrDefault(c => c.OrderId == orderId);
            order.ProductIds.Add(productId.ToString());
            dbContext.Orders.Update(order);
            await dbContext.SaveChangesAsync();

        }
        public async Task AddOrderToQueue(CreateOrderDTO orderDto, Guid userId)
        {
            Mapper mapper = AutoMapperUtil.ReturnMapper(new MapperConfiguration(con => con.CreateMap<CreateOrderDTO, Order>()));
            Order order = mapper.Map<Order>(orderDto);
            order.DateOfShipment = null;
            order.UserId = userId;
            order.DateOfOrder = DateTime.Now;

            string qName = Environment.GetEnvironmentVariable("CreateOrderQueue");
            string connString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            QueueClientOptions clientOpt = new QueueClientOptions() { MessageEncoding = QueueMessageEncoding.Base64 };

            QueueClient qClient = new QueueClient(connString, qName, clientOpt);
            var jsonOpt = new JsonSerializerOptions() { WriteIndented = true };
            string orderJson = JsonSerializer.Serialize<Order>(order, jsonOpt);
            await qClient.SendMessageAsync(orderJson);
        }
        public async Task CreateOrder(Order order)
        {
            await dbContext.Orders.AddAsync(order);
            await dbContext.SaveChangesAsync();
        }

        public async Task<GetOrderDTO> GetOrderByOrderId(Guid orderId)
        {
            await dbContext.SaveChangesAsync();

            Order o=  dbContext.Orders.FirstOrDefault(c => c.OrderId == orderId);
            return await GetOrderDTOConversion(o);
           
        }

        private async Task<GetOrderDTO> GetOrderDTOConversion(Order o)
        {
            List<Product> products = await ConvertProductIdsToObjects(o.ProductIds);

            return new GetOrderDTO
            {
                Products = products,
                DateOfOrder = o.DateOfOrder,
                DateOfShipment = o.DateOfShipment,
                Remarks = o.Remarks,
                IsOrderSent = o.IsOrderSent
            };
        }

        private async Task<List<Product>> ConvertProductIdsToObjects(List<string> productIds)
        {
            List<Product> allProducts = new List<Product>();
            foreach(string s in productIds)
            {
                allProducts.Add(await productService.GetProductById(Guid.Parse(s)));
            }
            return allProducts;
        }

        public async Task<IEnumerable<GetOrderDTO>> GetOrdersOfUser(Guid userId)
        {
            await dbContext.SaveChangesAsync();

           List<Order> orders= dbContext.Orders.Where(c => c.UserId == userId).ToList();

           List<GetOrderDTO>ordersInNicePrint = new List<GetOrderDTO>();
            foreach (Order o in orders)
            {
                ordersInNicePrint.Add(await GetOrderDTOConversion(o));
            }
            return ordersInNicePrint;
        }

        public async Task<bool> CheckIfOrderExist(Guid orderId)
        {
            await dbContext.SaveChangesAsync();
            if (dbContext.Orders.Count(x => x.OrderId == orderId) > 0)
                return true;
            else
                return false;
        }

        public async Task DeleteProductInOrder(Guid orderId, Guid productId)
        {
            Order order = dbContext.Orders.FirstOrDefault(c => c.OrderId == orderId);

            foreach(string s in order.ProductIds)
            {
                if (s == productId.ToString())
                {
                    order.ProductIds.Remove(s);
                    break;
                }
            }
             dbContext.Orders.Update(order);
            await dbContext.SaveChangesAsync();
        }
        public async Task<Order> ReturnFullOrderObjectById(Guid orderId)
        {
            return dbContext.Orders.FirstOrDefault(o => o.OrderId == orderId);
        }

        public async Task CheckoutAndShipOrder(Order o)
        {
            o.IsOrderSent = true;
            o.DateOfShipment = DateTime.Now;
            dbContext.Orders.Update(o);
            await dbContext.SaveChangesAsync();
            
        }

        public async Task<bool> CheckIfOrderExistAndNotShipped(Guid orderId)
        {
            await dbContext.SaveChangesAsync();
            if (dbContext.Orders.Where(sh => sh.IsOrderSent == false).Count(x => x.OrderId == orderId) > 0)
                return true;
            else
                return false;
        }
    }
}
