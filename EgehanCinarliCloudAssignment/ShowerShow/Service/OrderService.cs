using AutoMapper;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using UserAndOrdersFunction.DTO;
using UserAndOrdersFunction.Model;
using UserAndOrdersFunction.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserAndOrdersFunction.Service
{
    public class OrderService : IOrderService
    {
        private IOrderRepository orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository; 
        }

        public async Task AddProductToOrder(Guid orderId, Guid productId)
        {
            await orderRepository.AddProductToOrder(orderId, productId);
        }

        public async Task CreateOrder(CreateOrderDTO orderDTO, Guid userId)
        {
            await orderRepository.CreateOrder(orderDTO, userId);
        }

        public async Task<Order> GetOrderById(Guid orderId)
        {
            return await orderRepository.GetOrderById(orderId);
        }

        public async Task<IEnumerable<Order>> GetOrderByUser(Guid userId)
        {
            return await orderRepository.GetOrderByUser(userId);
        }

        public async Task RemoveProductFromOrder(Guid orderId, Guid productId)
        {
            await orderRepository.RemoveProductFromOrder(orderId, productId);
        }

        public async Task ShipOrder(Guid orderId)
        {
            await orderRepository.ShipOrder(orderId);
        }
    }
}
