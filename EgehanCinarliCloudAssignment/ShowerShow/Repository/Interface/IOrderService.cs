using UserAndOrdersFunction.DTO;
using UserAndOrdersFunction.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserAndOrdersFunction.Repository.Interface
{
    public interface IOrderService
    {
         Task CreateOrder(Order order);
         Task CreateProductInOrder(Guid orderId, Guid productId);
         Task DeleteProductInOrder(Guid orderId, Guid productId);
         Task CheckoutAndShipOrder(Guid orderId);
         Task<bool> CheckIfOrderExist(Guid orderId);
         Task<GetOrderDTO> GetOrderByOrderId(Guid orderId);
         Task AddOrderToQueue(CreateOrderDTO orderDto, Guid userId);
         Task<IEnumerable<GetOrderDTO>> GetOrdersOfUser(Guid userId);

    }
}