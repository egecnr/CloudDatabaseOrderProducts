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
        private IUserService userService;
        private IProductService productService;

        public OrderService(IOrderRepository orderRepository, IUserService userService, IProductService productService)
        {
            this.orderRepository = orderRepository;
            this.userService = userService;
            this.productService = productService;
        }

        public async Task CreateProductInOrder(Guid orderId, Guid productId)
        {
            if (!await productService.CheckIfProductExist(productId))
            {
                throw new Exception("Product doesnt exist");
            }else if(!await orderRepository.CheckIfOrderExist(orderId))
            {
                throw new Exception($"Order with id {orderId} does not exist");
            }
            else if (!await orderRepository.CheckIfOrderExistAndNotShipped(orderId))
            {
                throw new Exception($"Order with id {orderId} has already been shipped");
            }
            else
            {
                await orderRepository.CreateProductInOrder(orderId, productId);
            }
        }

        public async Task CreateOrder(Order order)
        {
            await orderRepository.CreateOrder(order);
        }

        public async Task<GetOrderDTO> GetOrderByOrderId(Guid orderId)
        {
            if(await orderRepository.CheckIfOrderExist(orderId))
            {
                return await orderRepository.GetOrderByOrderId(orderId);
            }
            else
            {
                throw new Exception("Order doesnt exist");
            }
        }

        public async Task<IEnumerable<GetOrderDTO>> GetOrdersOfUser(Guid userId)
        {
            if (await userService.CheckIfUserExist(userId))
            {
                return await orderRepository.GetOrdersOfUser(userId);

            }
            else
            {
                throw new Exception($"User with the id {userId} doesn't exist");
            }
        }

        public async Task DeleteProductInOrder(Guid orderId, Guid productId)
        {
            if (!await productService.CheckIfProductExist(productId))
            {
                throw new Exception("Product doesnt exist");
            }
            else if (!await orderRepository.CheckIfOrderExist(orderId))
            {
                throw new Exception($"Order with id {orderId} does not exist");
            }
            else if (!await orderRepository.CheckIfOrderExistAndNotShipped(orderId))
            {
                throw new Exception($"Order with id {orderId} has already been shipped");
            }
            else
            {
                await orderRepository.DeleteProductInOrder(orderId, productId);

            }
        }

        public async Task CheckoutAndShipOrder(Guid orderId)
        {
            if(await orderRepository.CheckIfOrderExist(orderId))
            {
                Order o = await orderRepository.ReturnFullOrderObjectById(orderId);
                await orderRepository.CheckoutAndShipOrder(o);
            }
            else
            {
                throw new Exception($"The order with the id {orderId} does not exist");
            }
        }

        public async Task AddOrderToQueue(CreateOrderDTO orderDto, Guid userId)
        {
            if (await userService.CheckIfUserExist(userId))
            {
                await orderRepository.AddOrderToQueue(orderDto, userId); 
            }
            else
            {
                throw new Exception("User does not exist");
            }
        }

        public async Task<bool> CheckIfOrderExist(Guid orderId)
        {
            return await orderRepository.CheckIfOrderExist(orderId);
        }
    }
}
