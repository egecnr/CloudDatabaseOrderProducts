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
        private IProductRepository productRepository;


        public OrderRepository(DatabaseContext dbContext, IProductRepository productRepository)
        {
            this.dbContext = dbContext;
            this.productRepository = productRepository;
        }

        public async Task AddProductToOrder(Guid orderId, Guid productId)
        {
            Order order = await GetOrderById(orderId);
            Product product = await productRepository.GetProductById(productId);
            ProductDTO productDTO = new ProductDTO()
            {
                Name = product.Name,
                Description = product.Description,
                Stock = product.Stock--
            };
            await productRepository.UpdateProduct(productId, productDTO);

            if (order.allOrderItems.ContainsKey(productId.ToString()))
            {
                foreach (KeyValuePair<string, int> kvp in order.allOrderItems)
                {
                    if (kvp.Key == productId.ToString())
                    {
                        order.allOrderItems[kvp.Key] = kvp.Value + 1;
                        break;
                    }
                }
            }
            else
            {
                order.allOrderItems.Add(productId.ToString(), 1);
            }
        }

        public async Task CreateOrder(CreateOrderDTO orderDTO, Guid userId)
        {
            Mapper mapper = AutoMapperUtil.ReturnMapper(new MapperConfiguration(con => con.CreateMap<CreateOrderDTO, Order>()));
            Order order = mapper.Map<Order>(orderDTO);
            order.DateOfShipment = null;
            order.UserId = userId;
            await dbContext.Orders.AddAsync(order);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Order> GetOrderById(Guid orderId)
        {
            await dbContext.SaveChangesAsync();

            return dbContext.Orders.FirstOrDefault(c => c.OrderId == orderId);
        }

        public async Task<IEnumerable<Order>> GetOrderByUser(Guid userId)
        {
            await dbContext.SaveChangesAsync();

            return dbContext.Orders.Where(c => c.UserId == userId).ToList();
        }

        public async Task RemoveProductFromOrder(Guid orderId, Guid productId)
        {
            Order order = await GetOrderById(orderId);
            Product product = await productRepository.GetProductById(productId);
            ProductDTO productDTO = new ProductDTO()
            {
                Name = product.Name,
                Description = product.Description,
                Stock = product.Stock++
            };
            await productRepository.UpdateProduct(productId, productDTO);
            if (order.allOrderItems.ContainsKey(productId.ToString()))
            {
                foreach (KeyValuePair<string, int> kvp in order.allOrderItems)
                {
                    if (kvp.Key == productId.ToString())
                    {
                        order.allOrderItems[kvp.Key] = kvp.Value - 1;
                        break;
                    }
                }
            }
            else
            {
                throw new Exception("Cannot remove product");
            }
        }

        public Task ShipOrder(Guid orderId)
        {
          
            throw new NotImplementedException();
        }
    }
}
